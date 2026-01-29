using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public class NoteMover : MonoBehaviour
    {
        public NoteType Type { get; private set; }
        public float Position { get; private set; }
        public bool Hit { get; private set; }

        private RectTransform _rectTransform;
        private float _speed;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Configure(NoteType type, float speed)
        {
            Type = type;
            _speed = speed;
            Position = -70f;
            Hit = false;
            UpdateVisual();
        }

        public void MarkHit()
        {
            Hit = true;
        }

        public void Tick(float deltaTime)
        {
            if (Hit)
            {
                return;
            }

            Position += _speed * deltaTime;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            if (!_rectTransform)
            {
                return;
            }

            var anchored = _rectTransform.anchoredPosition;
            anchored.x = -70f + Position;
            _rectTransform.anchoredPosition = anchored;
        }
    }
}
