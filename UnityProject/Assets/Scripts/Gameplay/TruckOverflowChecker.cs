using System.Collections.Generic;
using UnityEngine;

namespace AtomicParcel.Gameplay
{
    public class TruckOverflowChecker : MonoBehaviour
    {
        public Transform TruckRoot;
        public float TruckWidth = 2.4f;
        public float TruckHeight = 2.8f;
        public float TruckLength = 10f;
        public float FloorY = 0.85f;
        public float DoorZ = 2.2f;
        public int TruckCapacity = 28;
        public int OverflowLimit = 6;

        public bool IsOverflowing(List<Rigidbody> packages)
        {
            if (TruckRoot == null)
            {
                return false;
            }

            var loadedCount = 0;
            var overflowCount = 0;

            foreach (var body in packages)
            {
                if (body == null)
                {
                    continue;
                }

                var pos = TruckRoot.InverseTransformPoint(body.position);
                if (IsInsideTruck(pos))
                {
                    loadedCount++;
                }

                if (IsOverflowing(pos))
                {
                    overflowCount++;
                }
            }

            return loadedCount >= TruckCapacity && overflowCount >= OverflowLimit;
        }

        private bool IsInsideTruck(Vector3 pos)
        {
            return pos.z < DoorZ
                && pos.z > -TruckLength + 2.4f
                && Mathf.Abs(pos.x) < TruckWidth / 2f + 0.2f
                && pos.y > FloorY - 0.3f;
        }

        private bool IsOverflowing(Vector3 pos)
        {
            var heightLimit = TruckHeight + FloorY + 0.4f;
            return pos.y > heightLimit || pos.z > DoorZ + 0.4f;
        }
    }
}
