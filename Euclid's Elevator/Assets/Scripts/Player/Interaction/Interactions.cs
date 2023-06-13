using UnityEngine;

public class Interactions : MonoBehaviour
{
    public bool PickUp(CallType type, Player player, RaycastHit hit)
    {
        if (type == CallType.Started && hit.transform.TryGetComponent(out Item item))
        {
            player.inventory.PickUpItem(item);
            return true;
        }
        return false;
    }

    public bool ToggleDoor(CallType type, Player player, RaycastHit hit)
    {
        if (type == CallType.Started)
        {
            if (hit.collider.GetComponentInParent<Door>() != null)
            {
                Door door = hit.collider.GetComponentInParent<Door>();
                if (door.Locked)
                {
                    if (door.TryUnlock(player))
                    {
                        GameManager.Instance.enemy.InspectNoise(transform.position);
                        door.Toggle();
                        return true;
                    }
                    player.lockpick.PickLock(door);
                }
                else
                {
                    if (!door.Open && door.MatchesRequirement(player))
                    {
                        player.lockpick.Lock(door);
                        return true;
                    }
                    GameManager.Instance.enemy.InspectNoise(transform.position);
                    door.Toggle();
                }
                return true;
            }
        }
        else if (type == CallType.Canceled)
        {
            player.lockpick.Stop();
        }
        return false;
    }

    public bool InsertKeycard(CallType type, Player player, RaycastHit hit)
    {
        if (type == CallType.Started && hit.collider.CompareTag("ItemHole") && hit.collider.GetComponentInParent<Elevator>() != null)
        {
            Elevator elev = hit.collider.GetComponentInParent<Elevator>();

            if (elev.TryInsert(player))
            {
                return true;
            }
        }
        return false;
    }
}