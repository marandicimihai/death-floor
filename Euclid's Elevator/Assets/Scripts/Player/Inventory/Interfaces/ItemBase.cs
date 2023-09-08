using UnityEngine;

namespace DeathFloor.Inventory
{
    public class ItemBase : SyncValues, IItem
    {
        public ItemProperties Properties => _properties;

        [SerializeField] private ItemProperties _properties;

        private int _uses;

        private void Start()
        {
            _uses = _properties.Uses;
        }

        public void DecreaseDurability(out bool destroyed)
        {
            _uses--;
            destroyed = false;

            if (_uses <= 0)
            {
                OnBreak();
                destroyed = true;
                Destroy(gameObject);
            }
        }

        protected virtual void OnBreak()
        {

        }

        public GameObject GetRoot()
        {
            return gameObject;
        }
    }
}
