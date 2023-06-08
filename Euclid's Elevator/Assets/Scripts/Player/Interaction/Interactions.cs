using UnityEngine;

public class Interactions : MonoBehaviour
{
    [Header("Pick up")]
    [SerializeField] LayerMask items;

    [Header("Toggle Door")]
    [SerializeField] LayerMask door;
    [SerializeField] ItemProperties key;
    
    public bool PickUp(CallType type, Player player, RaycastHit hit)
    {
        if (type == CallType.Started && ((1 << hit.transform.gameObject.layer) & items.value) > 0 && hit.transform.TryGetComponent(out Item item))
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
            if (((1 << hit.transform.gameObject.layer) & door.value) > 0 && hit.collider.GetComponentInParent<Door>() != null)
            {
                Door door = hit.collider.GetComponentInParent<Door>();
                if (door.Locked)
                {
                    int i = 0;
                    foreach (Item item in player.inventory.Items)
                    {
                        if (item != null && door.CheckItem(item.properties))
                        {
                            player.inventory.DecreaseDurability(i);
                            door.Toggle();
                            return true;
                        }
                        i++;
                    }
                    player.lockpick.PickLock(door);
                }
                else
                {
                    if (!door.Open)
                    {
                        int i = 0;
                        foreach (Item item in player.inventory.Items)
                        {
                            if (item != null && item.properties.name == key.name)
                            {
                                player.lockpick.Lock(door, i);
                                return true;
                            }
                            i++;
                        }
                    }
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

            for (int i = 0; i < player.inventory.Items.Length; i++)
            {
                if (player.inventory.Items[i] != null && elev.CheckItem(player.inventory.Items[i].properties))
                {
                    player.inventory.DecreaseDurability(i);
                    return true;
                }
            }
        }
        return false;
    }
}