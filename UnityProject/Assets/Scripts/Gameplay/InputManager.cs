using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public class InputManager : MonoBehaviour
    {
        public KeyCode CubeKey = KeyCode.A;
        public KeyCode SphereKey = KeyCode.P;
        public KeyCode PizzaKey = KeyCode.Space;
        public KeyCode UltimateKey = KeyCode.E;

        public GameFlow GameFlow;
        public ScoreManager ScoreManager;
        public UltimateManager UltimateManager;

        private bool _cubePressed;
        private bool _spherePressed;
        private bool _pizzaPressed;

        private void Update()
        {
            if (!GameFlow || !GameFlow.IsGameplayActive)
            {
                return;
            }

            HandleKeyStates();

            if (Input.GetKeyDown(UltimateKey))
            {
                UltimateManager?.TryActivateUltimate();
            }

            if (Input.GetKeyDown(CubeKey))
            {
                ScoreManager?.TryHandlePrimaryHit(NoteType.Cube, _cubePressed, _spherePressed, _pizzaPressed);
            }

            if (Input.GetKeyDown(SphereKey))
            {
                ScoreManager?.TryHandlePrimaryHit(NoteType.Sphere, _cubePressed, _spherePressed, _pizzaPressed);
            }

            if (Input.GetKeyDown(PizzaKey))
            {
                ScoreManager?.TryHandlePrimaryHit(NoteType.Pizza, _cubePressed, _spherePressed, _pizzaPressed);
            }
        }

        private void HandleKeyStates()
        {
            _cubePressed = Input.GetKey(CubeKey);
            _spherePressed = Input.GetKey(SphereKey);
            _pizzaPressed = Input.GetKey(PizzaKey);
        }
    }
}
