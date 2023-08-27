using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class CollectableItemInteraction : MonoBehaviour, IInteractionHelper
    {
        public bool CheckInteractable(IInteractable interactable)
        {
            return true;
        }

        public void OnInteract()
        {
            
        }
    }
}