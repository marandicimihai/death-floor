using UnityEngine.InputSystem;
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

    [RequireInterface(typeof(IBehaviourService))] 
    [SerializeField] MonoBehaviour behaviourService;

    IBehaviourService Service => behaviourService as IBehaviourService;

    IInteractable currentInteracting;

    string interactInput;

    private void Awake()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.OnSettingsChanged += (Settings settings) =>
            {
                interactInput = Input.InputActions.General.Interact.controls[0].displayName;
            };
        }
        else
        {
            Debug.Log("No save system");
        }

        if (Input.InputActions != null)
        {
            Input.InputActions.General.Interact.performed += Interact;
            Input.InputActions.General.Interact.canceled += CancelInteract;
        }
        else
        {
            Debug.Log("No input class.");
        }

        //AVAILABLE INTERACTIONS ATM
        /*Interactions inter = GetComponent<Interactions>();
        interactions = new interaction[]
        {
            inter.PickUp,
            inter.ToggleDoor,
            inter.InsertInElevator,
            inter.EnterBox
        };*/
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
            interactable.OnInteractPerformed(Service);
            currentInteracting = interactable;
        }
    }

    void CancelInteract(InputAction.CallbackContext context)
    {
        if (currentInteracting != null && currentInteracting.IsInteractable)
        {
            currentInteracting.OnInteractCanceled(Service);
        }
    }

    public bool GetInteractionRaycast(out RaycastHit hit)
    {
        bool a = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitRay, interactionDistance, interactionLayerMask);
        hit = hitRay;
        return a;
    }
}
