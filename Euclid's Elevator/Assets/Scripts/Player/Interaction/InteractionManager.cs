using UnityEngine.InputSystem;
using UnityEngine;

public enum CallType
{
    Started,
    Canceled
}

[RequireComponent(typeof(Interactions))]
public class InteractionManager : MonoBehaviour
{
    [SerializeField] new Camera camera;
    [SerializeField] LayerMask interactionLayerMask;
    [SerializeField] float interactionDistance;

    Player player;
    delegate bool interaction(CallType type, Player player, RaycastHit hit);
    interaction[] interactions;

    private void Awake()
    {
        player = GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Interactions cannot be executed without the presence of the player script!");
        }

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

    void Interact(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactionDistance, interactionLayerMask))
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
        Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactionDistance, interactionLayerMask);
        
        foreach (interaction t in interactions)
        {
            if (t.Invoke(CallType.Canceled, player, hit))
            {
                break;
            }
        }
    }

    public RaycastHit GetInteractionRaycast()
    {
        Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactionDistance, interactionLayerMask);
        return hit;
    }
}
