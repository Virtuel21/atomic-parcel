using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public class UltimateManager : MonoBehaviour
    {
        public GameState GameState;
        public float UltimateDurationSeconds = 10f;
        public int UltimateChargeRequired = 10;

        public void AddUltimateCharge(int points)
        {
            if (GameState == null || GameState.UltimateActive)
            {
                return;
            }

            GameState.UltimateChargePoints = Mathf.Min(
                UltimateChargeRequired,
                GameState.UltimateChargePoints + points
            );

            GameState.UltimateCharge = Mathf.Min(
                100f,
                (GameState.UltimateChargePoints / (float)UltimateChargeRequired) * 100f
            );
        }

        public void TryActivateUltimate()
        {
            if (GameState == null || GameState.UltimateActive || GameState.UltimateCharge < 100f)
            {
                return;
            }

            GameState.UltimateActive = true;
            GameState.UltimateTimer = UltimateDurationSeconds;
            GameState.UltimatesUsed += 1;
            GameState.UltimateCharge = 100f;
            GameState.UltimateChargePoints = 0;
        }

        private void Update()
        {
            if (GameState == null || !GameState.UltimateActive)
            {
                return;
            }

            GameState.UltimateTimer -= Time.deltaTime;
            GameState.UltimateCharge = Mathf.Clamp01(GameState.UltimateTimer / UltimateDurationSeconds) * 100f;

            if (GameState.UltimateTimer <= 0f)
            {
                GameState.UltimateActive = false;
                GameState.UltimateCharge = 0f;
                GameState.UltimateChargePoints = 0;
                GameState.PerfectStreak = 0;
            }
        }
    }
}
