using UnityEngine.InputSystem;
using DeathFloor.SaveSystem;
using UnityEngine;

public enum CallType
{
    Started,
    Canceled
}

public class InteractionManager : MonoBehaviour
{
    [SerializeField] ActionInfoHUD info;
    [SerializeField] new Camera camera;
    [SerializeField] LayerMask interactionLayerMask;
    [SerializeField] float interactionDistance;

    IInteractable currentInteracting;

    string interactInput;

    private void Awake()
    {
        SaveSystem.OnSettingsChanged += (Settings settings) =>
        {
            interactInput = Input.Instance.InputActions.General.Interact.controls[0].displayName;
        };

        Input.Instance.InputActions.General.Interact.performed += Interact;
        Input.Instance.InputActions.General.Interact.canceled += CancelInteract;
    }

    private void Update()
    {
        if (GetInteractionRaycast(out RaycastHit hit))
        {
            if (info != null)
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable) && interactable.IsInteractable)
                {
                    string toDisplay = interactable.InteractionPrompt();
                    if (interactInput != string.Empty)
                    {
                        toDisplay += $" ({interactInput})";
                    }
                    info.SetActionText(toDisplay);
                }
            }
            else
            {
                Debug.Log("No hud.");
            }
            if (currentInteracting != null)
            {
                if (!hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    CancelInteract(new InputAction.CallbackContext());
                }
                else if (interactable != currentInteracting)
                {
                    CancelInteract(new InputAction.CallbackContext());
                }
            }
        }
        else
        {
            if (currentInteracting != null && currentInteracting.IsInteractable)
            {
                CancelInteract(new InputAction.CallbackContext());
            }
            if (info != null)
            {
                info.SetActionText(string.Empty);
            }
            else
            {
                Debug.Log("No hud.");
            }
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (GetInteractionRaycast(out RaycastHit hit) &&
            hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable) &&
            interactable.IsInteractable)
        {
            CancelInteract(new InputAction.CallbackContext());
            interactable.OnInteractPerformed();
            currentInteracting = interactable;
        }
    }

    void CancelInteract(InputAction.CallbackContext context)
    {
        if (currentInteracting != null && currentInteracting.IsInteractable)
        {
            currentInteracting.OnInteractCanceled();
        }
    }

    public bool GetInteractionRaycast(out RaycastHit hit)
    {
        bool a = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitRay, interactionDistance, interactionLayerMask);
        hit = hitRay;
        return a;
    }
}
