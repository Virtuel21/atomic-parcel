using System.Collections.Generic;
using UnityEngine;

namespace AtomicParcel.Audio
{
    public class AudioBeatDetector : MonoBehaviour
    {
        public AudioSource AudioSource;
        public int FftSize = 1024;
        public float Sensitivity = 1.15f;
        public float MinIntervalSeconds = 0.18f;
        public int HistorySize = 45;

        private readonly List<float> _energyHistory = new();
        private float _lastBeatTime;
        private float _prevBeatTime;

        public bool DetectBeat(out float bpm, out float intervalMs)
        {
            bpm = 0f;
            intervalMs = 0f;

            if (AudioSource == null)
            {
                return false;
            }

            var spectrum = new float[FftSize];
            var waveform = new float[FftSize];
            AudioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            AudioSource.GetOutputData(waveform, 0);

            var lowBins = Mathf.Min(18, spectrum.Length);
            var freqEnergy = 0f;
            for (var i = 0; i < lowBins; i++)
            {
                freqEnergy += spectrum[i];
            }
            freqEnergy /= Mathf.Max(1, lowBins);

            var rms = 0f;
            for (var i = 0; i < waveform.Length; i++)
            {
                rms += waveform[i] * waveform[i];
            }
            rms = Mathf.Sqrt(rms / waveform.Length);
            var rmsEnergy = rms * 120f;

            var energy = freqEnergy * 0.65f + rmsEnergy * 0.35f;

            _energyHistory.Add(energy);
            if (_energyHistory.Count > HistorySize)
            {
                _energyHistory.RemoveAt(0);
            }

            var avg = 0f;
            foreach (var sample in _energyHistory)
            {
                avg += sample;
            }
            avg /= Mathf.Max(1, _energyHistory.Count);

            var variance = 0f;
            foreach (var sample in _energyHistory)
            {
                variance += Mathf.Pow(sample - avg, 2f);
            }
            variance /= Mathf.Max(1, _energyHistory.Count);

            var dynamicThreshold = avg + Mathf.Sqrt(variance) * Sensitivity;
            var threshold = Mathf.Max(dynamicThreshold, avg * 1.05f + 6f);

            var now = AudioSource.time;
            if (energy > threshold && (now - _lastBeatTime) > MinIntervalSeconds)
            {
                _prevBeatTime = _lastBeatTime == 0f ? now : _lastBeatTime;
                _lastBeatTime = now;

                var interval = (_lastBeatTime - _prevBeatTime) * 1000f;
                if (interval > 100f)
                {
                    intervalMs = interval;
                    bpm = 60000f / interval;
                }
                return true;
            }

            return false;
        }
    }
}
