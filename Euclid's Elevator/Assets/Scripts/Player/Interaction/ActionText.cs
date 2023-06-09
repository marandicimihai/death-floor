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

    [Header("Items")]
    [SerializeField] string pickUpItem;

    public bool OpenDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked && 
            !hit.transform.GetComponentInParent<Door>().Locked && !hit.transform.GetComponentInParent<Door>().Open)
        {
            player.HUDManager.actionInfo.SetActionText(openDoor);
            return true;
        }
        return false;
    }

    public bool CloseDoor(Player player, RaycastHit hit)
    {
        if (hit.transform.GetComponentInParent<Door>() && !hit.transform.GetComponentInParent<Door>().StageLocked &&
            !hit.transform.GetComponentInParent<Door>().Locked && hit.transform.GetComponentInParent<Door>().Open)
        {
            player.HUDManager.actionInfo.SetActionText(closeDoor);
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
                player.HUDManager.actionInfo.SetActionText(lockDoor);
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
            player.HUDManager.actionInfo.SetActionText(pickLock);
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
            player.HUDManager.actionInfo.SetActionText(unlockDoor);
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
                player.HUDManager.actionInfo.SetActionText(insertKeycard);
                return true;
            }
        }
        return false;
    }

    public bool PickUpItem(Player player, RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out Item item))
        {
            player.HUDManager.actionInfo.SetActionText(pickUpItem);
            return true;
        }
        return false;
    }
}
