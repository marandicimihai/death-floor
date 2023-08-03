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

    private void Awake()
    {
        if (player == null)
        {
            Debug.Log("No player class.");
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
        }
        else
        {
            player.SetActionText(string.Empty);
        }
    }

    public bool GetInteractionRaycast(out RaycastHit hit)
    {
        bool a = Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hitRay, interactionDistance, interactionLayerMask);
        hit = hitRay;
        return a;
    }
}
