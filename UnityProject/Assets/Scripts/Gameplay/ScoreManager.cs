using System.Collections.Generic;
using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("References")]
        public GameState GameState;
        public UltimateManager UltimateManager;
        public PerformanceMeter PerformanceMeter;

        [Header("Timing")]
        public float HitZone = 300f;
        public float HitWindow = 40f;
        public float GoodWindow = 70f;
        public float OkWindow = 100f;

        [Header("Combo")]
        public int MissPenalty = 50;
        public int FragileMultiplier = 3;
        public float FragileDuration = 8f;

        private readonly List<NoteMover> _activeNotes = new();

        public void RegisterNote(NoteMover note)
        {
            if (note != null && !_activeNotes.Contains(note))
            {
                _activeNotes.Add(note);
            }
        }

        public void UnregisterNote(NoteMover note)
        {
            _activeNotes.Remove(note);
        }

        public void TryHandlePrimaryHit(NoteType type, bool cubePressed, bool spherePressed, bool pizzaPressed)
        {
            if (GameState == null || GameState.UltimateActive)
            {
                return;
            }

            if ((cubePressed && spherePressed) && TryHandleCombo(NoteType.Fragile))
            {
                return;
            }

            if ((cubePressed && pizzaPressed) && TryHandleCombo(NoteType.Express))
            {
                return;
            }

            if (!TryHandleHit(type))
            {
                ApplyMissPenalty();
            }
        }

        private bool TryHandleCombo(NoteType comboType)
        {
            return TryHandleHit(comboType);
        }

        private bool TryHandleHit(NoteType type)
        {
            var target = FindClosestNote(type);
            if (target == null)
            {
                return false;
            }

            var distance = Mathf.Abs(target.Position - HitZone);
            if (distance > OkWindow)
            {
                return false;
            }

            target.MarkHit();
            GameState.HitNotes += 1;
            PerformanceMeter?.RecordHit(true);

            var points = CalculatePoints(distance, type);
            ApplyComboSuccess(points);
            if (type == NoteType.Fragile)
            {
                TriggerFragileBonus();
            }
            else if (type == NoteType.Express)
            {
                TriggerExpressBonus();
            }

            return true;
        }

        private NoteMover FindClosestNote(NoteType type)
        {
            NoteMover best = null;
            var bestDistance = float.MaxValue;

            foreach (var note in _activeNotes)
            {
                if (note == null || note.Hit || note.Type != type)
                {
                    continue;
                }

                var distance = Mathf.Abs(note.Position - HitZone);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = note;
                }
            }

            return best;
        }

        private int CalculatePoints(float distance, NoteType type)
        {
            var basePoints = type switch
            {
                NoteType.Fragile => 70,
                NoteType.Express => 80,
                _ => 50
            };

            if (distance < HitWindow)
            {
                return basePoints;
            }

            if (distance < GoodWindow)
            {
                return Mathf.RoundToInt(basePoints * 0.6f);
            }

            return Mathf.RoundToInt(basePoints * 0.2f);
        }

        private void ApplyComboSuccess(int points)
        {
            GameState.Combo += 1;
            GameState.MaxCombo = Mathf.Max(GameState.MaxCombo, GameState.Combo);
            GameState.Multiplier = Mathf.Min(8, 1 + Mathf.FloorToInt(GameState.Combo / 5f));
            GameState.ComboPower = Mathf.Min(4f, 1f + Mathf.Floor(GameState.Combo / 10f));

            GameState.Score += Mathf.RoundToInt(points * GetEffectiveScoreMultiplier());
            UltimateManager?.AddUltimateCharge(1);
        }

        private void ApplyMissPenalty()
        {
            GameState.Combo = 0;
            GameState.PerfectStreak = 0;
            GameState.Multiplier = 1;
            GameState.ComboPower = 1f;
            GameState.Score = Mathf.Max(0, GameState.Score - MissPenalty);
            PerformanceMeter?.RecordHit(false);
        }

        private float GetEffectiveScoreMultiplier()
        {
            return GameState.Multiplier * GameState.FragileMultiplier;
        }

        private void TriggerFragileBonus()
        {
            GameState.FragileMultiplier = FragileMultiplier;
            GameState.FragileTimer = FragileDuration;
        }

        private void TriggerExpressBonus()
        {
            GameState.Score += Mathf.RoundToInt(75 * GetEffectiveScoreMultiplier());
        }
    }
}
