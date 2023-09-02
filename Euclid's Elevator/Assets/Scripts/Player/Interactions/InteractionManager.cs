using DeathFloor.Input;
using DeathFloor.Utilities;
using DeathFloor.Utilities.Logger;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class InteractionManager : MonoBehaviour, IToggleable
    {
        [Header("Input")]
        [SerializeField] private InputReader _inputReader;

        [Header("Logger")]
        [SerializeField] private bool _enableLogging;

        [Header("Optional")]
        [SerializeField, RequireInterface(typeof(IRaycastProvider))] private Object _raycastProvider;

        private bool _canInteract;

        private IInteractionHelper[] _helpers;

        private IRaycastProvider _raycastProviderInterface;
        private Utilities.Logger.ILogger _logger;

        private void Start()
        {
            _helpers = GetComponents<IInteractionHelper>();

            _logger ??= new DefaultLogger();

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
            //EXTRACT TO SEPARATE CLASS
            if (_raycastProviderInterface.GetRaycast(out RaycastHit hitInfo))
            {
                if (hitInfo.transform.TryGetComponent(out IInteractable interactable) && interactable.IsInteractable)
                {
                    string toDisplay = interactable.InteractionPrompt;

                    toDisplay += $" ({_inputReader.GetInteractionInputString()})";

                    //DISPLAY ON HUD
                }
                else
                {
                    //CLEAR THE HUD
                }
            }
            else
            {
                //CLEAR THE HUD
            }
        }

        private void Interact()
        {
            if (!_canInteract) return;

            if (_raycastProviderInterface.GetRaycast(out RaycastHit hitInfo) &&
                hitInfo.transform.TryGetComponent(out IInteractable interactable) &&
                interactable.IsInteractable)
            {
                _logger.ToggleLogging(_enableLogging)
                       .Log($"Interacted with {hitInfo.transform.name}");
                
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
