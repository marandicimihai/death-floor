using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class Inventory : MonoBehaviour
    {
        [SerializeField] private Transform _itemParent;
        [SerializeField] private int _itemCount;

        private CollectableItem[] _items;
        private int _index;

        private void Start()
        {
            _items = new CollectableItem[_itemCount];
        }

        public void PickUp(CollectableItem item)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    var newItem = Instantiate(item.Properties.HoldingModel, _itemParent);
                    newItem.transform.SetPositionAndRotation(item.Properties.OffsetPosition, Quaternion.Euler(item.Properties.OffsetRotation));
                    newItem.GetComponent<CollectableItem>().SetValuesRuntime(item);

                    Destroy(item.gameObject);

                    break;
                }
            }
        }
    }
}