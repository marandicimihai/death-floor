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
    [SerializeField] Transform holdPosition;
    [SerializeField] int inventoryCapacity;
    [SerializeField] float throwForce;
    [SerializeField] float swaySmooth;
    [SerializeField] float swaySmoothRot;
    [SerializeField] float swayStep;
    [SerializeField] float swayMaxStep;
    [SerializeField] float swayRotStep;
    [SerializeField] float swayMaxRotStep;

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

    private void Update()
    {
        Sway();
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
        for (int i = 0; i < holdPosition.childCount; i++)
        {
            Destroy(holdPosition.GetChild(i).gameObject);
        }
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
            items[i] = null;
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

        RefreshHolding();

        StartCoroutine(RefreshItems());
    }
    
    void RefreshHolding()
    {
        for (int i = 0; i < holdPosition.childCount; i++)
        {
            Destroy(holdPosition.GetChild(i).gameObject);
        }
        if (items[ActiveSlot] != null)
        {
            GameObject newObj = Instantiate(items[ActiveSlot].itemObj.holdPrefab, holdPosition);
            newObj.transform.localPosition = items[ActiveSlot].itemObj.holdOffset;
            newObj.transform.localEulerAngles = items[ActiveSlot].itemObj.holdAngles;
        }
    }

    void Sway()
    {
        if (items[ActiveSlot] != null && holdPosition.childCount > 0)
        {
            Vector2 invertLook = controller.PlayerInputActions.General.Look.ReadValue<Vector2>() * -swayStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -swayMaxStep, swayMaxStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -swayMaxStep, swayMaxStep);

            Vector3 swayPos = invertLook;

            Vector3 swayRotLook = controller.PlayerInputActions.General.Look.ReadValue<Vector2>() * -swayRotStep;
            swayRotLook.x = Mathf.Clamp(swayRotLook.x, -swayMaxRotStep, swayMaxRotStep);
            swayRotLook.y = Mathf.Clamp(swayRotLook.y, -swayMaxRotStep, swayMaxRotStep);

            Vector3 swayRot = new Vector3(swayRotLook.y, swayRotLook.x, swayRotLook.x);

            holdPosition.GetChild(0).localPosition = Vector3.Lerp(holdPosition.GetChild(0).localPosition, swayPos + items[ActiveSlot].itemObj.holdOffset, Time.deltaTime * swaySmooth);
            holdPosition.GetChild(0).localRotation = Quaternion.Slerp(holdPosition.GetChild(0).localRotation, Quaternion.Euler(swayRot + items[ActiveSlot].itemObj.holdAngles), Time.deltaTime * swaySmoothRot);
        }
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
