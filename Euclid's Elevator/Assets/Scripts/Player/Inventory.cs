using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(FpsController))]
public class Inventory : MonoBehaviour
{
    public Item[] Items 
    {
        get
        {
            return items;
        }
    }

    [SerializeField] FpsController controller;
    [SerializeField] GameObject inventoryParent;
    [SerializeField] Item[] items;
    [SerializeField] int inventoryCapacity;
    [SerializeField] float throwForce;

    [Header("UI")]
    [SerializeField] Image[] frames;
    [SerializeField] GameObject[] itemImagesObjects;
    [SerializeField] Image[] itemImages;
    [SerializeField] Sprite emptyFrame;
    [SerializeField] Sprite selectedFrame;

    public int ActiveSlot { get; private set; }


    private void Start()
    {
        items = new Item[inventoryCapacity];

        PlayerInputActions actions = controller.PlayerInputActions;
        actions.General.Inventory1.started += InventoryPerformed;
        actions.General.Inventory2.started += InventoryPerformed;
        actions.General.Inventory3.started += InventoryPerformed;
        actions.General.Inventory4.started += InventoryPerformed;
        RefreshInventoryScreen();
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

        if (ActiveSlot != input && items[input] != null && items[input].itemObj != null && items[input].itemObj.drawSounds != null)
        {
            SoundManager.Instance.PlaySound(items[input].itemObj.drawSounds);
        }
        ActiveSlot = input;
        RefreshInventoryScreen();
    }

    public void PickUpItem(Item item)
    {
        if (FindSlot(out int index))
        {
            Item newItem = inventoryParent.AddComponent(item.GetType()) as Item;
            newItem.SetValues(item);
            items[index] = newItem;
            Destroy(item.gameObject);
            if (newItem.itemObj != null && newItem.itemObj.pickUpSounds != null)
            {
                SoundManager.Instance.PlaySound(newItem.itemObj.pickUpSounds);
            }
            RefreshInventoryScreen();
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
            newItem.GetComponent<Item>().PlayDropSound();
            if (newItem.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(throwForward * throwForce, ForceMode.Impulse);
            }
            Destroy(items[ActiveSlot]);
            items[ActiveSlot] = null;
            RefreshInventoryScreen();
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
        foreach (Item item in items)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        items = new Item[inventoryCapacity];
        ActiveSlot = 0;
        RefreshInventoryScreen();
    }

    public void UseItem(int i)
    {
        items[i].uses -= 1;
        if (items[i].itemObj.useSounds != null)
        {
            if (items[i] != null && items[i].itemObj != null && items[i].itemObj.useSoundsInOrder)
            {
                SoundManager.Instance.PlaySounds(items[i].itemObj.useSounds);
            }
            else if (items[i] != null && items[i].itemObj != null)
            {
                SoundManager.Instance.PlaySound(items[i].itemObj.useSounds);
            }
        }
        if (items[i].uses <= 0)
        {
            Destroy(items[i]);
        }
        RefreshInventoryScreen();
    }

    void RefreshInventoryScreen()
    {
        for (int i = 0; i < frames.Length; i++)
        {
            if (i == ActiveSlot)
            {
                frames[i].sprite = selectedFrame; 
            }
            else
            {
                frames[i].sprite = emptyFrame;
            }
        }
        StartCoroutine(RefreshItems());
    }
    
    IEnumerator RefreshItems()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (items[i] != null)
            {
                itemImagesObjects[i].SetActive(true);
                itemImages[i].sprite = items[i].itemObj.inventoryIcon;
            }
            else
            {
                itemImagesObjects[i].SetActive(false);
                itemImages[i].sprite = null;
            }
        }
    }
}
