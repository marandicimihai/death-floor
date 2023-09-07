using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class InteractionRedirector : MonoBehaviour, IInteractable
    {
        public InteractionTag Tag { get => _redirectToInterface.Tag; }

        public bool IsInteractable => true;

        public string InteractionPrompt => _redirectToInterface?.InteractionPrompt;

        [SerializeField, RequireInterface(typeof(IInteractable))] private Object _redirectTo;

        private IInteractable _redirectToInterface;

        private void Start()
        {
            _redirectToInterface = _redirectTo as IInteractable;
        }

        public void Interact() => _redirectToInterface?.Interact();

        public GameObject GetRoot() => _redirectToInterface.GetRoot();
    }
}