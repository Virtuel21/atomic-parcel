using System.Collections.Generic;
using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public class PerformanceMeter : MonoBehaviour
    {
        public GameState GameState;
        public float WindowMs = 20000f;
        public float RequiredAccuracy = 0.05f;
        public float DangerDurationMs = 20000f;
        public int MinNotes = 6;

        private readonly List<RecentNoteResult> _recent = new();

        public void RecordHit(bool hit)
        {
            if (GameState == null)
            {
                return;
            }

            _recent.Add(new RecentNoteResult { Time = Time.time * 1000f, Hit = hit });
            PruneRecent();
        }

        private void Update()
        {
            if (GameState == null || !GameState.IsPlaying)
            {
                return;
            }

            UpdatePerformance(Time.deltaTime * 1000f);
        }

        private void UpdatePerformance(float deltaMs)
        {
            PruneRecent();
            var total = _recent.Count;
            var hits = 0;
            foreach (var entry in _recent)
            {
                if (entry.Hit)
                {
                    hits += 1;
                }
            }

            if (total >= MinNotes)
            {
                var accuracy = total == 0 ? 1f : hits / (float)total;
                if (accuracy < RequiredAccuracy)
                {
                    GameState.DangerTimer += deltaMs;
                }
                else
                {
                    GameState.DangerTimer = Mathf.Max(0f, GameState.DangerTimer - deltaMs * 1.5f);
                }
            }
            else
            {
                GameState.DangerTimer = Mathf.Max(0f, GameState.DangerTimer - deltaMs);
            }
        }

        private void PruneRecent()
        {
            var cutoff = (Time.time * 1000f) - WindowMs;
            _recent.RemoveAll(entry => entry.Time < cutoff);
        }
    }
}
