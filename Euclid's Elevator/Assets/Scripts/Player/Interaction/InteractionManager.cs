using UnityEngine.InputSystem;
using UnityEngine;

public enum CallType
{
    Started,
    Canceled
}

[RequireComponent(typeof(Interactions))]
[RequireComponent(typeof(ActionText))]
public class InteractionManager : MonoBehaviour
{
    [SerializeField] new Camera camera;
    [SerializeField] LayerMask interactionLayerMask;
    [SerializeField] float interactionDistance;

    Player player;
    delegate bool interaction(CallType type, Player player, RaycastHit hit);
    interaction[] interactions;

    delegate bool action(Player player, RaycastHit hit);
    action[] actions;

    private void Awake()
    {
        player = GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Interactions cannot be executed without the presence of the player script!");
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
            acte.InsertKeycard,
            acte.PickUpItem
        };

        Interactions inter = GetComponent<Interactions>();
        interactions = new interaction[]
        {
            inter.PickUp,
            inter.ToggleDoor,
            inter.InsertKeycard
        };
    }

    private void Start()
    {
        Input.InputActions.General.Interact.performed += Interact;
        Input.InputActions.General.Interact.canceled += InteractionCanceled;
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
                player.HUDManager.actionInfo.SetActionText(string.Empty);
            }
        }
        else
        {
            player.HUDManager.actionInfo.SetActionText(string.Empty);
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (GetInteractionRaycast(out RaycastHit hit))
        {
            foreach (interaction t in interactions)
            {
                if (t.Invoke(CallType.Started, player, hit))
                {
                    break;
                }
            }
        }
    }

    void InteractionCanceled(InputAction.CallbackContext context)
    {
        GetInteractionRaycast(out RaycastHit hit);
        
        foreach (interaction t in interactions)
        {
            if (t.Invoke(CallType.Canceled, player, hit))
            {
                break;
            }
        }
    }

    public bool GetInteractionRaycast(out RaycastHit hit)
    {
        bool a = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitRay, interactionDistance, interactionLayerMask);
        hit = hitRay;
        return a;
    }
}
