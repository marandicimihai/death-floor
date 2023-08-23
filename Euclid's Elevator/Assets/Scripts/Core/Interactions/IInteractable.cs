using UnityEngine;

namespace DeathFloor.Interactions
{
    public interface IInteractable
    {
        public bool IsInteractable { get; }
        public string InteractionPrompt { get; }
        public void Interact();
    }
}
