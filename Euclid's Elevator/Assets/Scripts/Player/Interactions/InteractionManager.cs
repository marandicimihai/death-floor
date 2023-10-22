using DeathFloor.HUD;
using DeathFloor.Input;
using DeathFloor.Utilities;
using System;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class InteractionManager : MonoBehaviour, IToggleable
    {
        [Header("Input")]
        [SerializeField] private InputReader _inputReader;

        [Header("Other")]
        [SerializeField, RequireInterface(typeof(IRaycastProvider))] private UnityEngine.Object _raycastProvider;
        [SerializeField, RequireInterface(typeof(IActionInfo))] private UnityEngine.Object _actionInfo;

        private bool _canInteract;

        private IInteractable _current;
        private IInteractionHelper[] _helpers;

        private IActionInfo _actionInfoInterface;
        private IRaycastProvider _raycastProviderInterface;
        private void Start()
        {
            _helpers = GetComponents<IInteractionHelper>();

            _actionInfoInterface = _actionInfo as IActionInfo;
            _raycastProviderInterface = _raycastProvider as IRaycastProvider;

            _inputReader.StartInteract += Interact;
            _inputReader.EndInteract += EndInteract;

            Enable();
        }

        private void OnDisable()
        {
            _inputReader.StartInteract -= Interact;
            _inputReader.EndInteract -= EndInteract;
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
                _current = interactable;

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

        private void EndInteract()
        {
            if (!_canInteract || _current == null) return;

            foreach (IInteractionHelper helper in _helpers)
            {
                try
                {
                    if (_current?.Tag == helper.TargetInteractionTag)
                    {
                        helper.EndInteractionExtension(_current?.GetRoot());
                    }
                }
                catch (MissingReferenceException mre)
                {
                    Debug.Log("The object that was interacted with has been destroyed.");
                    Debug.Log(mre.Message);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
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
