using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public Item[] Items 
    {
        get
        {
            return items;
        }
        set
        {
            items = value;
        }
    }
    Item[] items;

    public int Index { get; private set; }

    [SerializeField] int slots;
    [SerializeField] Transform inventory;
    
    [Header("Drop Properties")]
    [SerializeField] Transform dropPoint;
    [SerializeField] float dropForce;

    public EventHandler OnItemsChanged;

    private void Awake()
    {
        Items = new Item[slots];
    }

    private void Start()
    {
        Input.InputActions.General.Drop.performed += (InputAction.CallbackContext context) => DropItem();
    }

    public void PickUpItem(Item itemComponent)
    {
        for (int i = 0; i < slots; i++)
        {
            if (Items[i] == null)
            {
                Item newItem = inventory.gameObject.AddComponent<Item>();
                newItem.SetValues(itemComponent);

                Destroy(itemComponent.gameObject);

                Items[i] = newItem;
                OnItemsChanged?.Invoke(this, new EventArgs());
                return;
            }
        }
    }

    void DropItem()
    {
        if (Items[Index] == null)
            return;

        GameObject dropped = Instantiate(Items[Index].properties.physicalObject, dropPoint.position, Quaternion.identity);

        if (dropped.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(dropPoint.forward * dropForce, ForceMode.Impulse);
        }
        if (dropped.TryGetComponent(out Item item))
        {
            item.SetValues(Items[Index]);
        }

        Destroy(Items[Index]);
        Items[Index] = null;

        OnItemsChanged?.Invoke(this, new EventArgs());
    }

    void UseItem()
    {
        if (Items[Index] != null && Items[Index].OnUse())
        {
            Items[Index].uses -= 1;

            if (Items[Index].uses <= 0)
            {
                Destroy(Items[Index]);
                Items[Index] = null;
                OnItemsChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
