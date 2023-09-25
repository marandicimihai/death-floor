using DeathFloor.Interactions;
using UnityEngine;

namespace DeathFloor.Inventory
{
    public class CollectableItem : ItemBase, IInteractable
    {
        public InteractionTag Tag => _interactionTag;

        public bool IsInteractable => _isInteractable;

        public string InteractionPrompt => _interactionPrompt;

        [SerializeField] private InteractionTag _interactionTag;
        [SerializeField] private bool _isInteractable;
        [SerializeField] private string _interactionPrompt;

        public void Interact()
        {

        }
    }
}
