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
        interactInput = Input.InputActions.General.Interact.controls[0].displayName;
        SaveSystem.Instance.OnSettingsChanged += (Settings settings) =>
        {
            interactInput = Input.InputActions.General.Interact.controls[0].displayName;
        };
    }

    public bool OpenDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked && 
            !hit.transform.GetComponentInParent<Door>().Locked && !hit.transform.GetComponentInParent<Door>().Open)
        {
            player.HUDManager.actionInfo.SetActionText(openDoor + $" ({interactInput})");
            return true;
        }
        return false;
    }

    public bool CloseDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked &&
            !hit.transform.GetComponentInParent<Door>().Locked && hit.transform.GetComponentInParent<Door>().Open)
        {
            player.HUDManager.actionInfo.SetActionText(closeDoor + $" ({interactInput})");
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
                player.HUDManager.actionInfo.SetActionText(lockDoor + $" ({interactInput})");
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
            player.HUDManager.actionInfo.SetActionText(pickLock + $" ({interactInput})");
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
            player.HUDManager.actionInfo.SetActionText(unlockDoor + $" ({interactInput})");
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
                player.HUDManager.actionInfo.SetActionText(insertKeycard + $" ({interactInput})");
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
                player.HUDManager.actionInfo.SetActionText(repairElevator + $" ({interactInput})");
                return true;
            }
        }
        return false;
    }

    public bool PickUpItem(Player player, RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out Item item))
        {
            player.HUDManager.actionInfo.SetActionText(pickUpItem + $" ({interactInput})");
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
                player.HUDManager.actionInfo.SetActionText(hide + $" ({interactInput})");
            }
            return true;
        }
        return false;
    }
}
