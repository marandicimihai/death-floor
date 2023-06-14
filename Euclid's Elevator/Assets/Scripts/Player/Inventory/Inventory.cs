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
    [SerializeField] Player player;

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
        Input.InputActions.General.Use.performed += (InputAction.CallbackContext context) => UseItem();
        Input.InputActions.General.Inventory1.performed += InventoryPerformed;
        Input.InputActions.General.Inventory2.performed += InventoryPerformed;
        Input.InputActions.General.Inventory3.performed += InventoryPerformed;
        Input.InputActions.General.Inventory4.performed += InventoryPerformed;
    }

    void InventoryPerformed(InputAction.CallbackContext context)
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

        if (input != Index && Items[Index] != null)
        {
            AudioManager.Instance.PlayRandomClip(Items[Index].properties.holster);
        }
        Index = input;
        OnItemsChanged?.Invoke(this, new EventArgs());
    }

    public void PickUpItem(Item itemComponent)
    {
        for (int i = 0; i < slots; i++)
        {
            if (Items[i] == null)
            {
                Item newItem = inventory.gameObject.AddComponent(itemComponent.GetType()) as Item;
                newItem.SetValues(itemComponent);

                Destroy(itemComponent.gameObject);

                AudioManager.Instance.PlayRandomClip(newItem.properties.pickup);

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
            AudioManager.Instance.PlayRandomClip(dropped, item.properties.drop);
        }

        Destroy(Items[Index]);
        Items[Index] = null;

        OnItemsChanged?.Invoke(this, new EventArgs());
    }

    void UseItem()
    {
        if (Items[Index] != null && Items[Index].TryGetComponent(out IUsable usable) && usable.OnUse(player))
        {
            if (Items[Index].properties.useUseSoundsInOrder)
            {
                AudioManager.Instance.PlayClips(Items[Index].properties.use);
            }    
            else
            {
                AudioManager.Instance.PlayRandomClip(Items[Index].properties.use);
            }

            DecreaseDurability();
        }
    }

    public void DecreaseDurability(int id = -1)
    {
        if (id == -1)
        {
            id = Index;
        }

        if (Items[id] != null)
        {
            Items[id].uses -= 1;

            if (Items[id].uses <= 0)
            {
                Destroy(Items[id]);
                Items[id] = null;
                OnItemsChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < slots; i++)
        {
            if (Items[i] != null)
            {
                Destroy(Items[i]);
                Items[i] = null;

                OnItemsChanged?.Invoke(this, new EventArgs());
            }
        }
    }

    public bool Contains(ItemProperties prop)
    {
        for (int i = 0; i < slots; i++)
        {
            if (Items[i] != null && Items[i].properties.name == prop.name)
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemIndex(ItemProperties prop)
    {
        for (int i = 0; i < slots; i++)
        {
            if (Items[i] != null && Items[i].properties.name == prop.name)
            {
                return i;
            }
        }
        return -1;
    }
}
