using DeathFloor.Input;
using DeathFloor.Utilities;
using DeathFloor.Utilities.Logger;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class InteractionManager : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputReader _inputReader;

        [Header("Logger")]
        [SerializeField, Tooltip("Updates on Start")] private bool _enableLogging;

        [Header("Optional")]
        [SerializeField] private Optional<MonoBehaviour> _raycastProviderBehaviour;

        private IOptionalAssigner _optionalAssigner;
        private IRaycastProvider _raycastProvider;
        private Utilities.Logger.ILogger _logger;

        private void Start()
        {
            _logger ??= new DefaultLogger(_enableLogging);
            _optionalAssigner ??= new OptionalAssigner(this);

            _raycastProvider = _optionalAssigner.AssignUsingGetComponent<IRaycastProvider>(_raycastProviderBehaviour);

            _inputReader.Interacted += Interact;
        }

        private void OnDisable()
        {
            _inputReader.Interacted -= Interact;
        }

        private void Update()
        {
            //EXTRACT TO SEPARATE CLASS
            if (_raycastProvider.GetRaycast(out RaycastHit hitInfo))
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
            if (_raycastProvider.GetRaycast(out RaycastHit hitInfo) &&
                hitInfo.transform.TryGetComponent(out IInteractable interactable) &&
                interactable.IsInteractable)
            {
                _logger.ToggleLogging(_enableLogging)
                       .Log($"Interacted with {hitInfo.transform.name}");
                interactable.Interact();
            }
        }
    }
}
