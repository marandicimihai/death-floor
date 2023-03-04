using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FpsController))]
public class Inventory : MonoBehaviour
{
    public Item[] Items { get => items; }

    [SerializeField] FpsController controller;
    [SerializeField] Item[] items;
    [SerializeField] GameObject inventoryParent;
    [SerializeField] int inventoryCapacity;
    [SerializeField] float throwForce;

    public int ActiveSlot { get; private set; }


    private void Start()
    {
        items = new Item[inventoryCapacity];

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
            Item newItem = inventoryParent.AddComponent<Item>();
            newItem.SetValues(item);
            items[index] = newItem;
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
            GameObject newItem = Instantiate(items[ActiveSlot].itemObj.prefab, throwPoint, Quaternion.identity);
            newItem.GetComponent<Item>().SetValues(items[ActiveSlot]);
            if (newItem.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(throwForward * throwForce, ForceMode.Impulse);
            }
            Destroy(items[ActiveSlot]);
            items[ActiveSlot] = null;
        }
    }

    bool FindSlot(out int slotIndex)
    {
        slotIndex = 0;

        foreach (Item item in items)
        {
            if (item == null)
            {
                return true;
            }
            slotIndex++;
        }

        return false;
    }

    public void Die()
    {
        items = new Item[inventoryCapacity];
        ActiveSlot = 0;
    }

    public void UseItem(int i)
    {
        items[i].uses -= 1;
        if (items[i].uses <= 0)
        {
            Destroy(items[i]);
        }
    }
}
