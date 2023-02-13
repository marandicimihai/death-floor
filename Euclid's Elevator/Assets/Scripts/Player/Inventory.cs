using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FpsController))]
public class Inventory : MonoBehaviour
{
    [SerializeField] ItemObject[] items;
    [SerializeField] int inventoryCapacity;
    [SerializeField] float throwForce;

    public int ActiveSlot { get; private set; }

    FpsController controller;

    private void Start()
    {
        items = new ItemObject[inventoryCapacity];

        controller = GetComponent<FpsController>();
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
            Instantiate(items[ActiveSlot].prefab, throwPoint, Quaternion.identity).TryGetComponent<Rigidbody>(out Rigidbody rb);
            if (rb != null)
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
