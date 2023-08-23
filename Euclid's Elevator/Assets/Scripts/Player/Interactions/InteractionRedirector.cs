using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class InteractionRedirector : MonoBehaviour, IInteractable
    {
        public bool IsInteractable => true;

        public string InteractionPrompt => _redirectTo?.InteractionPrompt;

        [SerializeField] private MonoBehaviour _redirectToBehaviour;

        private IInteractable _redirectTo;

        private void Start()
        {
            _redirectTo = _redirectToBehaviour as IInteractable;
        }

        public void Interact() => _redirectTo?.Interact();
    }
}