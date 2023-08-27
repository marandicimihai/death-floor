using DeathFloor.Interactions;
using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class CollectableItem : SyncValues, IInteractable
    {
        public bool IsInteractable => _isInteractable;
        public string InteractionPrompt => _interactionPrompt;
        public ItemProperties Properties => _properties;

        [SerializeField] private ItemProperties _properties;
        [SerializeField] private bool _isInteractable;
        [SerializeField] private string _interactionPrompt;

        public void Interact()
        {
            //PICK UP
        }
    }
}
