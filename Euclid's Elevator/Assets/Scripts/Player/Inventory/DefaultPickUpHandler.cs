using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class DefaultPickUpHandler : MonoBehaviour, IPickupHandler
    {
        [SerializeField] private Transform _itemParent;

        public CollectableItem PickUp(CollectableItem item)
        {
            var newItem = Instantiate(item.Properties.HoldingModel, _itemParent);
            newItem.transform.SetLocalPositionAndRotation(item.Properties.OffsetPosition, Quaternion.Euler(item.Properties.OffsetRotation));

            if (newItem.TryGetComponent(out CollectableItem collectable))
            {
                collectable.SetValuesRuntime(item);
            }

            Destroy(item.gameObject);
            return collectable;
        }
    }
}