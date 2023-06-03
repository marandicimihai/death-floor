using UnityEngine;

public class Interactions : MonoBehaviour
{
    [Header("Pick up")]
    [SerializeField] LayerMask items;
    
    public bool PickUp(Player player, RaycastHit hit)
    {
        if (((1 << hit.transform.gameObject.layer) & items.value) > 0 && hit.transform.TryGetComponent(out Item item))
        {
            player.inventory.PickUpItem(item);
            return true;
        }
        return false;
    }
}