using DeathFloor.HUD;
using DeathFloor.Input;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class InteractionManager : MonoBehaviour, IToggleable
    {
        [Header("Input")]
        [SerializeField] private InputReader _inputReader;

        [Header("Other")]
        [SerializeField, RequireInterface(typeof(IRaycastProvider))] private Object _raycastProvider;
        [SerializeField, RequireInterface(typeof(IActionInfo))] private Object _actionInfo;

        private bool _canInteract;

        private IInteractionHelper[] _helpers;

        private IActionInfo _actionInfoInterface;
        private IRaycastProvider _raycastProviderInterface;
        private void Start()
        {
            _helpers = GetComponents<IInteractionHelper>();

            _actionInfoInterface = _actionInfo as IActionInfo;
            _raycastProviderInterface = _raycastProvider as IRaycastProvider;

            _inputReader.Interacted += Interact;

            Enable();
        }

        private void OnDisable()
        {
            _inputReader.Interacted -= Interact;
        }

        private void Update()
        {
            if (_raycastProviderInterface != null && 
                _raycastProviderInterface.GetRaycast(out RaycastHit hitInfo))
            {
                if (hitInfo.transform.TryGetComponent(out IInteractable interactable) && interactable.IsInteractable)
                {
                    string toDisplay = interactable.InteractionPrompt;

                    if (!string.IsNullOrEmpty(toDisplay))
                    {
                        toDisplay += $" ({_inputReader.GetInteractionInputString()})";

                        _actionInfoInterface?.DisplayText(toDisplay);
                    }
                    else
                    {
                        _actionInfoInterface?.ClearText();
                    }
                }
                else
                {
                    _actionInfoInterface?.ClearText();
                }
            }
            else
            {
                _actionInfoInterface?.ClearText();
            }
        }

        private void Interact()
        {
            if (!_canInteract) return;

            if (_raycastProviderInterface.GetRaycast(out RaycastHit hitInfo) &&
                hitInfo.transform.TryGetComponent(out IInteractable interactable) &&
                interactable.IsInteractable)
            {
                foreach(IInteractionHelper helper in _helpers)
                {
                    if (interactable.Tag == helper.TargetInteractionTag)
                    {
                        helper.InteractionExtension(interactable.GetRoot());
                    }
                }

                interactable.Interact();
            }
        }

        public void Enable()
        {
            _canInteract = true;
        }

        public void Disable()
        {
            _canInteract = false;
        }
    }
}
