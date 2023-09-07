using DeathFloor.Input;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class InventoryManager : MonoBehaviour, IInventoryManager
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private int _itemCount;
        [SerializeField, RequireInterface(typeof(IInventoryDisplayer))] private Object _inventoryDisplayer;
        [SerializeField, RequireInterface(typeof(ISwayHandler))] private Object _swayHandler;
        [SerializeField, RequireInterface(typeof(IPickupHandler))] private Object _pickupHandler;
        [SerializeField, RequireInterface(typeof(IDropHandler))] private Object _dropHandler;

        private IInventoryDisplayer _displayer;
        private ISwayHandler _sway;
        private IPickupHandler _pickup;
        private IDropHandler _drop;

        private IUseHelper[] _helpers;
        private CollectableItem[] _items;
        private int _index;

        private void Start()
        {
            _items = new CollectableItem[_itemCount];

            _helpers = GetComponents<IUseHelper>();
            _displayer = _inventoryDisplayer as IInventoryDisplayer;
            _sway = _swayHandler as ISwayHandler;
            _pickup = _pickupHandler as IPickupHandler;
            _drop = _dropHandler as IDropHandler;

            _inputReader.Dropped += Drop;
            _inputReader.Inventory += SetActiveSlot;
            _inputReader.Scrolled += Scroll;
            _inputReader.Used += UseItem;
        }

        private void OnDisable()
        {
            _inputReader.Dropped -= Drop;
            _inputReader.Inventory -= SetActiveSlot;
            _inputReader.Scrolled -= Scroll;
            _inputReader.Used -= UseItem;
        }

        private void Update()
        {
            if (_items[_index] != null)
            {
                _sway?.PerformSway(_items[_index].gameObject, _items[_index].Properties);
            }
        }

        private void SetActiveSlot(int slot)
        {
            _index = slot;
            _displayer.RefreshView(_items, _index);
        }

        private void Scroll(float amount)
        {
            amount = Mathf.Clamp(amount, -1, 1);

            _index += (int)amount;
            _index = Mathf.Clamp(_index, 0, _itemCount - 1);

            _displayer.RefreshView(_items, _index);
        }

        public void PickUp(CollectableItem item)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                {
                    _items[i] = _pickup?.PickUp(item);

                    _displayer.RefreshView(_items, _index);

                    break;
                }
            }
        }

        private void Drop()
        {
            if (_items[_index] != null)
            {
                _drop?.DropItem(_items[_index]);

                _items[_index] = null;

                _displayer.RefreshView(_items, _index);
            }
        }

        public ItemProperties GetActiveItem()
        {
            if (_items != null && _items[_index] != null)
            {
                return _items[_index].Properties;
            }
            return null;
        }

        private void UseItem()
        {
            if (_items != null && _items[_index] != null && _items[_index].TryGetComponent(out IUsable usable))
            {
                foreach (IUseHelper helper in _helpers)
                {
                    if (helper.TargetUseTag == usable.UseTag)
                    {
                        helper.UseExtension(_items[_index].gameObject);
                    }
                }

                usable.OnUse();
                _items[_index].DecreaseDurability(out bool destroyed);

                if (destroyed)
                {
                    _items[_index] = null;
                    _displayer.RefreshView(_items, _index);
                }
            }
        }

        public void ClearInventory()
        {
            _items = new CollectableItem[_itemCount];
            _displayer.RefreshView(_items, _index);
        }

        public void DecreaseDurability()
        {
            if (_items != null && _items[_index] != null)
            {
                _items[_index].DecreaseDurability(out bool destroyed);

                if (destroyed)
                {
                    _items[_index] = null;
                    _displayer.RefreshView(_items, _index);
                }
            }
        }
    }
}