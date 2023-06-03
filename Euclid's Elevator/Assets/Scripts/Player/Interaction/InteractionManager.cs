using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Interactions))]
public class InteractionManager : MonoBehaviour
{
    [SerializeField] new Camera camera;
    [SerializeField] LayerMask interactionLayerMask;
    [SerializeField] float interactionDistance;

    Player player;
    delegate bool interaction(Player player, RaycastHit hit);
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
            inter.PickUp
        };
    }

    private void Start()
    {
        Input.InputActions.General.Interact.performed += Interact;
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, interactionDistance, interactionLayerMask))
        {
            foreach (interaction t in interactions)
            {
                if (t.Invoke(player, hit))
                {
                    break;
                }
            }
        }
    }
}
