using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class Inventory : MonoBehaviour
{
    public static EventHandler OnPickUpKeycard;
    public EventHandler OnItemsChanged;

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

    [SerializeField] ItemProperties[] scriptableobjects;

    [SerializeField] int slots;
    [SerializeField] Transform inventory;
    [SerializeField] Player player;

    [Header("Drop Properties")]
    [SerializeField] Transform dropPoint;
    [SerializeField] float dropForce;

    [Header("Events")]
    [SerializeField] ItemProperties keycard;

    //TO PREVENT HOLSTER OF ANOTHER ITEM PLAYING WHEN SWITCHED QUICKLY
    AudioJob currentHolster;

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
        Input.InputActions.General.Scroll.performed += Scroll;

        Input.InputActions.Box.Use.performed += (InputAction.CallbackContext context) => UseItem();
        Input.InputActions.Box.Inventory1.performed += InventoryPerformed;
        Input.InputActions.Box.Inventory2.performed += InventoryPerformed;
        Input.InputActions.Box.Inventory3.performed += InventoryPerformed;
        Input.InputActions.Box.Inventory4.performed += InventoryPerformed;
        Input.InputActions.Box.Scroll.performed += Scroll;
    }

    ItemProperties GetProperties(string name)
    {
        foreach (ItemProperties prop in scriptableobjects)
        {
            if (prop.name == name)
            {
                return prop;
            }
        }
        return null;
    }

    void Scroll(InputAction.CallbackContext context)
    {
        int scroll = (int)Mathf.Clamp(context.ReadValue<float>(), -1, 1);

        Index += scroll;

        if (Index >= slots)
        {
            Index = 0;
        }
        else if (Index < 0)
        {
            Index = slots - 1;
        }

        if (Items[Index] != null)
        {
            AudioManager.Instance.StopClip(currentHolster);
            currentHolster = AudioManager.Instance.PlayRandomClip(Items[Index].properties.holster);
        }
        OnItemsChanged?.Invoke(this, new EventArgs());
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

        if (input != Index && Items[input] != null)
        {
            AudioManager.Instance.StopClip(currentHolster);
            currentHolster = AudioManager.Instance.PlayRandomClip(Items[input].properties.holster);
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
                Item newItem = Instantiate(itemComponent.properties.inHandObject, inventory).GetComponent<Item>();
                newItem.SetValues(itemComponent);

                if (itemComponent.properties.name == keycard.name)
                {
                    OnPickUpKeycard?.Invoke(this, new EventArgs());
                }

                player.journal.AddPage(itemComponent.properties.page);
                player.HUDManager.popupHUD.PopUp(itemComponent.properties.popUp);

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
            ItemManager.spawnedItems.Add(item);
            AudioManager.Instance.PlayRandomClip(dropped, item.properties.drop);
        }

        Destroy(Items[Index].gameObject);
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

    public void DecreaseDurability(int id = -1, bool shouldPlayUseSounds = false)
    {
        if (id == -1)
        {
            id = Index;
        }

        if (shouldPlayUseSounds)
        {
            if (Items[id].properties.useUseSoundsInOrder)
            {
                AudioManager.Instance.PlayClips(Items[id].properties.use);
            }
            else
            {
                AudioManager.Instance.PlayRandomClip(Items[id].properties.use);
            }
        }

        if (Items[id] != null)
        {
            Items[id].DecreaseDurability();

            if (Items[id].uses <= 0)
            {
                Destroy(Items[id].gameObject);
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
                Destroy(Items[i].gameObject);
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
