using UnityEngine;
using UnityEngine.UI;

namespace AtomicParcel.UI
{
    public class HudController : MonoBehaviour
    {
        public AtomicParcel.Gameplay.GameState GameState;

        [Header("Text")]
        public Text ScoreText;
        public Text ComboText;
        public Text MultiplierText;
        public Text TimerText;

        [Header("Ultimate")]
        public Image UltimateFill;

        private void Update()
        {
            if (GameState == null)
            {
                return;
            }

            if (ScoreText) ScoreText.text = GameState.Score.ToString("N0");
            if (ComboText) ComboText.text = GameState.Combo.ToString();
            if (MultiplierText) MultiplierText.text = GameState.Multiplier.ToString();
            if (UltimateFill) UltimateFill.fillAmount = GameState.UltimateCharge / 100f;
        }
    }
}
