using DeathFloor.Interactions;
using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class CollectableItem : SyncValues, IInteractable
    {
        public InteractionTag Tag { get => _interactionTag; }
        public bool IsInteractable => _isInteractable;
        public string InteractionPrompt => _interactionPrompt;
        public ItemProperties Properties => _properties;

        [SerializeField] private ItemProperties _properties;
        [SerializeField] private InteractionTag _interactionTag;
        [SerializeField] private bool _isInteractable;
        [SerializeField] private string _interactionPrompt;

        private int _uses;

        private void Start()
        {
            _uses = _properties.Uses;
        }

        public void Interact()
        {

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
