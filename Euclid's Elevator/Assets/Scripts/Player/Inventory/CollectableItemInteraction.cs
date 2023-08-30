using DeathFloor.Inventory;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class CollectableItemInteraction : MonoBehaviour, IInteractionHelper
    {
        public InteractionTag TargetInteractionTag => _targetInteractionTag;

        [SerializeField] private InteractionTag _targetInteractionTag;
        [SerializeField] private Optional<MonoBehaviour> _inventoryBehaviour;

        private IOptionalAssigner _optionalAssigner;
        private IInventoryManager _inventory;

        private void Start()
        {
            _optionalAssigner = new OptionalAssigner(this);

            _inventory = _optionalAssigner.AssignUsingGetComponent<IInventoryManager>(_inventoryBehaviour);
        }

        public void InteractionExtension(GameObject rootObject)
        {
            if (rootObject.TryGetComponent(out CollectableItem item))
            {
                _inventory.PickUp(item);
            }
        }
    }
}