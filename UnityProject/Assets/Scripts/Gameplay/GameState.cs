using System.Collections.Generic;
using UnityEngine;

namespace AtomicParcel.Gameplay
{
    [System.Serializable]
    public class RecentNoteResult
    {
        public float Time;
        public bool Hit;
    }

    public class GameState : MonoBehaviour
    {
        [Header("Score")]
        public int Score;
        public int Combo;
        public int MaxCombo;
        public int Multiplier = 1;
        public int BrokenPackages;

        [Header("Accuracy")]
        public int TotalNotes;
        public int HitNotes;
        public int PerfectStreak;

        [Header("Ultimate")]
        public float UltimateCharge;
        public int UltimateChargePoints;
        public bool UltimateActive;
        public float UltimateTimer;
        public int UltimatesUsed;

        [Header("Rhythm")]
        public int Bpm = 120;
        public float BeatIntervalMs = 500f;
        public float LastBeatTime;

        [Header("Gameplay")]
        public float Power = 0.7f;
        public float ComboPower = 1f;
        public float FragileMultiplier = 1f;
        public float FragileTimer;
        public float DangerTimer;

        [Header("State")]
        public bool IsPlaying;
        public bool IsPaused;

        public readonly List<RecentNoteResult> RecentNoteResults = new();

        public void ResetState()
        {
            Score = 0;
            Combo = 0;
            MaxCombo = 0;
            Multiplier = 1;
            BrokenPackages = 0;
            TotalNotes = 0;
            HitNotes = 0;
            PerfectStreak = 0;
            UltimateCharge = 0f;
            UltimateChargePoints = 0;
            UltimateActive = false;
            UltimateTimer = 0f;
            UltimatesUsed = 0;
            Power = 0.7f;
            ComboPower = 1f;
            FragileMultiplier = 1f;
            FragileTimer = 0f;
            DangerTimer = 0f;
            IsPlaying = false;
            IsPaused = false;
            RecentNoteResults.Clear();
        }
    }
}
