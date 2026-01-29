using System.Collections.Generic;
using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public enum NoteType
    {
        Cube,
        Sphere,
        Pizza,
        Fragile,
        Express
    }

    public class NoteSpawner : MonoBehaviour
    {
        [Header("References")]
        public GameState GameState;
        public ScoreManager ScoreManager;
        public RectTransform NoteContainer;
        public GameObject CubeNotePrefab;
        public GameObject SphereNotePrefab;
        public GameObject PizzaNotePrefab;
        public GameObject FragileNotePrefab;
        public GameObject ExpressNotePrefab;

        [Header("Timing")]
        public float BaseNoteSpeed = 300f;
        public float NoteSpacing = 20f;
        public float NoteWidth = 60f;
        public float FragileNoteWidth = 140f;
        public float ExpressNoteWidth = 150f;

        [Header("Difficulty")]
        public int MinNotesPerSecond = 4;
        public int MaxNotesPerSecond = 5;

        private float _noteSpeed;
        private float _lastNoteSpawnTime;
        private float _lastNoteSpawnWidth;
        private float _lastComboNoteTime = float.NegativeInfinity;
        private float _lastRegularNoteTime = float.NegativeInfinity;
        private readonly Queue<float> _noteSpawnTimestamps = new();
        private readonly HashSet<NoteType> _comboNoteTypes = new() { NoteType.Fragile, NoteType.Express };

        private void Awake()
        {
            _lastNoteSpawnWidth = NoteWidth;
            UpdateNoteSpeed();
        }

        public void UpdateNoteSpeed()
        {
            var spacing = NoteWidth + NoteSpacing;
            _noteSpeed = MaxNotesPerSecond > 0
                ? Mathf.Max(BaseNoteSpeed, spacing * MaxNotesPerSecond)
                : BaseNoteSpeed;
        }

        public void ResetSpawner()
        {
            _noteSpawnTimestamps.Clear();
            _lastNoteSpawnTime = 0f;
            _lastNoteSpawnWidth = NoteWidth;
            _lastComboNoteTime = float.NegativeInfinity;
            _lastRegularNoteTime = float.NegativeInfinity;
        }

        public void Tick(float timeMs)
        {
            if (!GameState || !GameState.IsPlaying || GameState.IsPaused)
            {
                return;
            }

            SpawnBeatNotes(timeMs);
        }

        private void SpawnBeatNotes(float timeMs)
        {
            var spawnInterval = 60000f / Mathf.Max(1, GameState.Bpm) / 2f;
            var shouldForce = ShouldForceSpawn(timeMs);

            if (timeMs - _lastNoteSpawnTime > spawnInterval || shouldForce)
            {
                var shouldSpawn = shouldForce || Random.value > 0.25f;
                if (shouldSpawn && SpawnNoteIfAllowed(timeMs))
                {
                    _lastNoteSpawnTime = timeMs;
                }
            }
        }

        private bool SpawnNoteIfAllowed(float timeMs)
        {
            var nextType = PickNoteType();
            var nextWidth = GetNoteWidth(nextType);

            if (!CanSpawnNote(timeMs, nextType, nextWidth))
            {
                return false;
            }

            CreateBeatNote(nextType);
            RegisterNoteSpawn(timeMs, nextWidth, nextType);
            return true;
        }

        private NoteType PickNoteType()
        {
            var fragileChance = 0.1f;
            var expressChance = 0.08f;
            var roll = Random.value;

            if (roll < fragileChance)
            {
                return NoteType.Fragile;
            }

            if (roll < fragileChance + expressChance)
            {
                return NoteType.Express;
            }

            var baseTypes = new[] { NoteType.Cube, NoteType.Sphere, NoteType.Pizza };
            return baseTypes[Random.Range(0, baseTypes.Length)];
        }

        private float GetNoteWidth(NoteType type)
        {
            return type switch
            {
                NoteType.Fragile => FragileNoteWidth,
                NoteType.Express => ExpressNoteWidth,
                _ => NoteWidth
            };
        }

        private bool CanSpawnNote(float timeMs, NoteType nextType, float nextWidth)
        {
            var spacingInterval = GetSpawnSpacingInterval(nextWidth);
            while (_noteSpawnTimestamps.Count > 0 && timeMs - _noteSpawnTimestamps.Peek() > 1000f)
            {
                _noteSpawnTimestamps.Dequeue();
            }

            if (MaxNotesPerSecond > 0 && _noteSpawnTimestamps.Count >= MaxNotesPerSecond)
            {
                return false;
            }

            if (timeMs - _lastNoteSpawnTime < spacingInterval)
            {
                return false;
            }

            if (_comboNoteTypes.Contains(nextType))
            {
                if (timeMs - _lastRegularNoteTime < 240f)
                {
                    return false;
                }
            }
            else if (timeMs - _lastComboNoteTime < 240f)
            {
                return false;
            }

            return true;
        }

        private float GetSpawnSpacingInterval(float nextWidth)
        {
            var requiredSpacing = (_lastNoteSpawnWidth / 2f) + (nextWidth / 2f) + NoteSpacing;
            return (requiredSpacing / _noteSpeed) * 1000f;
        }

        private void RegisterNoteSpawn(float timeMs, float width, NoteType type)
        {
            _noteSpawnTimestamps.Enqueue(timeMs);
            _lastNoteSpawnTime = timeMs;
            _lastNoteSpawnWidth = width;
            if (_comboNoteTypes.Contains(type))
            {
                _lastComboNoteTime = timeMs;
            }
            else
            {
                _lastRegularNoteTime = timeMs;
            }
        }

        private bool ShouldForceSpawn(float timeMs)
        {
            if (MinNotesPerSecond <= 0)
            {
                return false;
            }

            var maxGap = 1000f / MinNotesPerSecond;
            return (timeMs - _lastNoteSpawnTime) >= maxGap;
        }

        private void CreateBeatNote(NoteType type)
        {
            var prefab = GetPrefabForType(type);
            if (!prefab || !NoteContainer)
            {
                return;
            }

            var instance = Instantiate(prefab, NoteContainer);
            var mover = instance.GetComponent<NoteMover>();
            if (mover != null)
            {
                mover.Configure(type, _noteSpeed);
                ScoreManager?.RegisterNote(mover);
            }

            GameState.TotalNotes += 1;
        }

        private GameObject GetPrefabForType(NoteType type)
        {
            return type switch
            {
                NoteType.Cube => CubeNotePrefab,
                NoteType.Sphere => SphereNotePrefab,
                NoteType.Pizza => PizzaNotePrefab,
                NoteType.Fragile => FragileNotePrefab,
                NoteType.Express => ExpressNotePrefab,
                _ => CubeNotePrefab
            };
        }
    }
}
