using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public class GameFlow : MonoBehaviour
    {
        public GameState GameState;
        public float DefaultGameDurationMs = 90000f;

        public bool IsGameplayActive => GameState != null && GameState.IsPlaying && !GameState.IsPaused;

        private float _gameTimeMs;
        private float _gameDurationMs;

        public void StartGame()
        {
            if (GameState == null)
            {
                return;
            }

            GameState.ResetState();
            GameState.IsPlaying = true;
            GameState.IsPaused = false;
            _gameTimeMs = 0f;
            _gameDurationMs = DefaultGameDurationMs;
        }

        public void PauseGame()
        {
            if (GameState == null)
            {
                return;
            }

            GameState.IsPaused = true;
        }

        public void ResumeGame()
        {
            if (GameState == null)
            {
                return;
            }

            GameState.IsPaused = false;
        }

        public void EndGame()
        {
            if (GameState == null)
            {
                return;
            }

            GameState.IsPlaying = false;
        }

        private void Update()
        {
            if (!IsGameplayActive)
            {
                return;
            }

            _gameTimeMs += Time.deltaTime * 1000f;
            if (_gameTimeMs >= _gameDurationMs)
            {
                EndGame();
            }
        }
    }
}
