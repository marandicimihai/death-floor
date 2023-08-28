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

        public void Interact()
        {

        }

        public GameObject GetRoot()
        {
            return gameObject;
        }
    }
}
