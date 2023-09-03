using DeathFloor.Inventory;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class CollectableItemInteraction : MonoBehaviour, IInteractionHelper
    {
        public InteractionTag TargetInteractionTag => _targetInteractionTag;

        [SerializeField] private InteractionTag _targetInteractionTag;
        [SerializeField, RequireInterface(typeof(IInventoryManager))] private Object _inventory;

        private IInventoryManager _inventoryInterface;

        private void Start()
        {
            _inventoryInterface = _inventory as IInventoryManager;
        }

        public void InteractionExtension(GameObject rootObject)
        {
            if (rootObject.TryGetComponent(out CollectableItem item))
            {
                _inventoryInterface?.PickUp(item);
            }
        }
    }
}