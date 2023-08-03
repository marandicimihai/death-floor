using UnityEngine;

public class ActionText : MonoBehaviour
{
    [Header("Door related")]
    [SerializeField] string openDoor;
    [SerializeField] string closeDoor;
    [SerializeField] string lockDoor;
    [SerializeField] string pickLock;
    [SerializeField] string unlockDoor;

    [Header("Elevator")]
    [SerializeField] string insertKeycard;
    [SerializeField] string repairElevator;

    [Header("Items")]
    [SerializeField] string pickUpItem;

    [Header("Box")]
    [SerializeField] string hide;
    [SerializeField] string getOut;

    string interactInput;

    private void Start()
    {
        if (Input.InputActions != null)
        {
            interactInput = Input.InputActions.General.Interact.controls[0].displayName;
        }
        else
        {
            Debug.Log("Input class absent");
        }

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
    }

    public bool OpenDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked && 
            !hit.transform.GetComponentInParent<Door>().Locked && !hit.transform.GetComponentInParent<Door>().Open)
        {
            player.SetActionText(openDoor + $" ({interactInput})");
            return true;
        }
        return false;
    }

    public bool CloseDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked &&
            !hit.transform.GetComponentInParent<Door>().Locked && hit.transform.GetComponentInParent<Door>().Open)
        {
            player.SetActionText(closeDoor + $" ({interactInput})");
            return true;
        }
        return false;
    }

    public bool LockDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked &&
            !hit.transform.GetComponentInParent<Door>().Locked && !hit.transform.GetComponentInParent<Door>().Open)
        {
            if (hit.transform.GetComponentInParent<Door>().MatchesRequirement(player))
            {
                player.SetActionText(lockDoor + $" ({interactInput})");
                return true;
            }
        }
        return false;
    }

    public bool PickLock(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked &&
            hit.transform.GetComponentInParent<Door>().Locked && !hit.transform.GetComponentInParent<Door>().Open)
        {
            player.SetActionText(pickLock + $" ({interactInput})");
            return true;
        }
        return false;
    }

    public bool UnlockDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked &&
            hit.transform.GetComponentInParent<Door>().Locked && !hit.transform.GetComponentInParent<Door>().Open &&
            hit.transform.GetComponentInParent<Door>().MatchesRequirement(player))
        {
            player.SetActionText(unlockDoor + $" ({interactInput})");
            return true;
        }
        return false;
    }

    public bool InsertKeycard(Player player, RaycastHit hit)
    {
        if (hit.collider.CompareTag("ItemHole") && hit.collider.GetComponentInParent<Elevator>() != null)
        {
            if (hit.collider.GetComponentInParent<Elevator>().MatchesRequirement(player))
            {
                player.SetActionText(insertKeycard + $" ({interactInput})");
                return true;
            }
        }
        return false;
    }

    public bool Repair(Player player, RaycastHit hit)
    {
        if (hit.collider.CompareTag("ItemHole") && hit.collider.GetComponentInParent<Elevator>() != null)
        {
            if (hit.collider.GetComponentInParent<Elevator>().Broken &&
                hit.collider.GetComponentInParent<Elevator>().MatchesRequirement(player))
            {
                player.SetActionText(repairElevator + $" ({interactInput})");
                return true;
            }
        }
        return false;
    }

    public bool PickUpItem(Player player, RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out Item item))
        {
            player.SetActionText(pickUpItem + $" ({interactInput})");
            return true;
        }
        return false;
    }

    public bool Hide(Player player, RaycastHit hit)
    {
        if (hit.collider.CompareTag("HidingBox") && hit.collider.TryGetComponent(out HidingBox box))
        {
            if (!box.hasPlayer)
            {
                player.SetActionText(hide + $" ({interactInput})");
            }
            return true;
        }
        return false;
    }
}
