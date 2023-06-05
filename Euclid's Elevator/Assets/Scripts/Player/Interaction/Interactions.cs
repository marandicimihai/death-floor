using UnityEngine;

public class Interactions : MonoBehaviour
{
    [Header("Pick up")]
    [SerializeField] LayerMask items;

    [Header("Toggle Door")]
    [SerializeField] LayerMask door;
    
    public bool PickUp(Player player, RaycastHit hit)
    {
        if (((1 << hit.transform.gameObject.layer) & items.value) > 0 && hit.transform.TryGetComponent(out Item item))
        {
            player.inventory.PickUpItem(item);
            return true;
        }
        return false;
    }

    public bool ToggleDoor(Player player, RaycastHit hit)
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
                        break;
                    }
                    i++;
                }
            }
            else
            {
                door.Toggle();
            }
            return true;
        }
        return false;
    }

    public bool InsertKeycard(Player player, RaycastHit hit)
    {
        if (hit.collider.CompareTag("ItemHole") && hit.collider.GetComponentInParent<Elevator>() != null)
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