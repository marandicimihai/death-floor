using DeathFloor.Door;
using DeathFloor.Inventory;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class DoorInteraction : MonoBehaviour, IInteractionHelper
    {
        public InteractionTag TargetInteractionTag => _targetTag;

        [SerializeField] private InteractionTag _targetTag;
        [SerializeField, RequireInterface(typeof(IInventoryManager))] private Object _inventoryManager;

        private IInventoryManager _inventory;

        public void InteractionExtension(GameObject rootObject)
        {
            if (!rootObject.TryGetComponent(out IDoor door))
                return;

            _inventory ??= _inventoryManager as IInventoryManager;

            bool successful = door.TryUnlock(_inventory?.GetActiveItem());

            if (successful)
                _inventory.DecreaseDurability();
        }
    }
}