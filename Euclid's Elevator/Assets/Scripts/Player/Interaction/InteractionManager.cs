using UnityEngine.InputSystem;
using UnityEngine;

public enum CallType
{
    Started,
    Canceled
}

[RequireComponent(typeof(ActionText))]
public class InteractionManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] new Camera camera;
    [SerializeField] LayerMask interactionLayerMask;
    [SerializeField] float interactionDistance;

    delegate bool action(Player player, RaycastHit hit);
    action[] actions;

    IInteractable currentInteracting;

    private void Awake()
    {
        if (player == null)
        {
            Debug.Log("No player class.");
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

        //sorted by priority
        ActionText acte = GetComponent<ActionText>();
        actions = new action[]
        {
            acte.LockDoor,
            acte.OpenDoor,
            acte.CloseDoor,
            acte.UnlockDoor,
            acte.PickLock,
            acte.Repair,
            acte.InsertKeycard,
            acte.PickUpItem,
            acte.Hide
        };

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
            bool gate = false;
            foreach (action ac in actions)
            {
                if (ac.Invoke(player, hit))
                {
                    gate = true;
                    break;
                }
            }
            if (!gate)
            {
                player.SetActionText(string.Empty);
            }

            if (currentInteracting != null)
            {
                if (!hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    currentInteracting.OnInteractCanceled(player, hit);
                }
                else if (interactable != currentInteracting)
                {
                    currentInteracting.OnInteractCanceled(player, hit);
                }
            }
        }
        else
        {
            if (currentInteracting != null)
            {
                currentInteracting.OnInteractCanceled(player, hit);
            }
            player.SetActionText(string.Empty);
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (GetInteractionRaycast(out RaycastHit hit) &&
            hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            interactable.OnInteractPerformed(player, hit);
            currentInteracting = interactable;
        }
    }

    void CancelInteract(InputAction.CallbackContext context)
    {
        GetInteractionRaycast(out RaycastHit hit);
        currentInteracting.OnInteractCanceled(player, hit);
    }

    public bool GetInteractionRaycast(out RaycastHit hit)
    {
        bool a = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitRay, interactionDistance, interactionLayerMask);
        hit = hitRay;
        return a;
    }
}
