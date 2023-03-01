using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FpsController))]
public class Inventory : MonoBehaviour
{
    public ItemObject[] Items { get => items; }

    [SerializeField] FpsController controller;
    [SerializeField] ItemObject[] items;
    [SerializeField] int inventoryCapacity;
    [SerializeField] float throwForce;

    public int ActiveSlot { get; private set; }


    private void Start()
    {
        items = new ItemObject[inventoryCapacity];

        PlayerInputActions actions = controller.PlayerInputActions;
        actions.General.Inventory.started += InventoryPerformed;
    }

    private void InventoryPerformed(InputAction.CallbackContext context)
    {
        int input;

        try
        {
            input = int.Parse(context.control.name) - 1;
        }
        catch
        {
            Debug.Log("Parse no work :c.");
            return;
        }

        ActiveSlot = input;
    }

    public void PickUpItem(Item item)
    {
        if (FindSlot(out int index))
        {
            items[index] = item.itemObj;
            Destroy(item.gameObject);
        }
        else
        {
            Debug.Log("No space in inventory");
        }
    }

    public void DropItem(Vector3 throwPoint, Vector3 throwForward)
    {
        if (items[ActiveSlot] != null)
        {
            if (Instantiate(items[ActiveSlot].prefab, throwPoint, Quaternion.identity).TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(throwForward * throwForce, ForceMode.Impulse);
            }
            items[ActiveSlot] = null;
        }
    }

    bool FindSlot(out int slotIndex)
    {
        slotIndex = 0;

        foreach(ItemObject item in items)
        {
            if (item == null)
            {
                return true;
            }
            slotIndex++;
        }

        return false;
    }
}
