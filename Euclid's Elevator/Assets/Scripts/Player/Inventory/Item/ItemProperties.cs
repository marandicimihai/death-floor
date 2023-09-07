using UnityEngine;

namespace DeathFloor.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item")]
    internal class ItemProperties : ScriptableObject
    {
        public int Uses { get => _uses; }
        public GameObject HoldingModel { get => _holdingModel; }
        public GameObject PhysicalModel { get => _physicalModel; }
        public Vector3 OffsetPosition { get => _offsetPosition; }
        public Vector3 OffsetRotation { get => _offsetRotation; }
        public Sprite Icon { get => _icon; }

        [SerializeField] private int _uses;
        [SerializeField] private GameObject _holdingModel;
        [SerializeField] private GameObject _physicalModel;
        [SerializeField] private Vector3 _offsetPosition;
        [SerializeField] private Vector3 _offsetRotation;
        [SerializeField] private Sprite _icon;
    }
}