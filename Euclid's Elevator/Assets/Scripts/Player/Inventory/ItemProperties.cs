using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class ItemProperties : ScriptableObject
    {
        public int Uses;
        public GameObject HoldingModel;
        public GameObject PhysicalModel;
        public Vector3 OffsetPosition;
        public Vector3 OffsetRotation;
    }
}