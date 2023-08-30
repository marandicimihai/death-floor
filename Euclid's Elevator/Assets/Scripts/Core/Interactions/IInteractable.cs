using UnityEngine;

namespace DeathFloor.Interactions
{
    public interface IInteractable
    {
        public InteractionTag Tag { get; }
        public bool IsInteractable { get; }
        public string InteractionPrompt { get; }
        public void Interact();
        public GameObject GetRoot();
    }
}
