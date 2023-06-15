using UnityEngine.UI;
using UnityEngine;
using System;

[RequireComponent(typeof(Inventory))]
public class InventoryHUD : MonoBehaviour
{
    [System.NonSerialized] public bool hideHUD;
    [Header("ItemModels")]
    [SerializeField] Transform holdPos;
    [SerializeField] float swayStep;
    [SerializeField] float swayMaxStep;
    [SerializeField] float swayRotStep;
    [SerializeField] float swayMaxRotStep;
    [SerializeField] float swaySmooth;
    [SerializeField] float swaySmoothRot;

    [Header("Slots")]
    [SerializeField] GameObject inventoryHUDParent;
    [SerializeField] Sprite selectedFrameIcon;
    [SerializeField] Sprite unselectedFrameIcon;
    [SerializeField] Image[] frames;
    [SerializeField] Image[] icons;

    Inventory inventory;
    bool wasHidden;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        inventory.OnItemsChanged += (object caller, EventArgs args) => RefreshVisualModels();
        inventory.OnItemsChanged += (object caller, EventArgs args) => RefreshInventoryHUD();
    }

    private void Update()
    {
        if (hideHUD)
        {
            inventoryHUDParent.SetActive(false);
            if (holdPos.childCount > 0)
            {
                DestroyChildren(holdPos);
            }
            wasHidden = true;
        }
        else
        {
            if (wasHidden)
            {
                RefreshVisualModels();
                wasHidden = false;
            }
            inventoryHUDParent.SetActive(true);
            Sway();
        }
    }

    void RefreshVisualModels()
    {
        if (hideHUD)
            return;

        if (holdPos.childCount > 0)
        {
            if (inventory.Items[inventory.Index] != null && holdPos.GetComponentInChildren<Item>() &&
                holdPos.GetComponentInChildren<Item>().properties.name != inventory.Items[inventory.Index].properties.name)
            {
                DestroyChildren(holdPos);
                Item item = inventory.Items[inventory.Index];
                GameObject model = Instantiate(item.properties.inHandObject, holdPos);
                model.transform.localPosition = item.properties.offset;
                model.transform.localEulerAngles = item.properties.holdAngles;
            }
            else if (inventory.Items[inventory.Index] == null)
            {
                DestroyChildren(holdPos);
            }
        }
        else
        {
            if (inventory.Items[inventory.Index] != null)
            {
                Item item = inventory.Items[inventory.Index];
                GameObject model = Instantiate(item.properties.inHandObject, holdPos);
                model.transform.localPosition = item.properties.offset;
                model.transform.localEulerAngles = item.properties.holdAngles;
            }
        }
    }

    void RefreshInventoryHUD()
    {
        for (int i = 0; i < frames.Length; i++)
        {
            if (i == inventory.Index)
            {
                frames[i].sprite = selectedFrameIcon;
            }
            else
            {
                frames[i].sprite = unselectedFrameIcon;
            }
        }
        for (int i = 0; i < icons.Length; i++)
        {
            if (inventory.Items[i] != null)
            {
                icons[i].sprite = inventory.Items[i].properties.icon;
                icons[i].gameObject.SetActive(true);
            }
            else
            {
                icons[i].gameObject.SetActive(false);
            }
        }
    }

    void Sway()
    {
        if (inventory.Items[inventory.Index] != null && holdPos.childCount > 0)
        {
            Vector2 invertLook = Input.InputActions.General.Look.ReadValue<Vector2>() * -swayStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -swayMaxStep, swayMaxStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -swayMaxStep, swayMaxStep);

            Vector3 swayPos = invertLook;

            Vector3 swayRotLook = Input.InputActions.General.Look.ReadValue<Vector2>() * -swayRotStep;
            swayRotLook.x = Mathf.Clamp(swayRotLook.x, -swayMaxRotStep, swayMaxRotStep);
            swayRotLook.y = Mathf.Clamp(swayRotLook.y, -swayMaxRotStep, swayMaxRotStep);

            Vector3 swayRot = new(swayRotLook.y, swayRotLook.x, swayRotLook.x);

            holdPos.GetChild(0).localPosition = Vector3.Lerp(holdPos.GetChild(0).localPosition, swayPos + inventory.Items[inventory.Index].properties.offset, Time.deltaTime * swaySmooth);
            holdPos.GetChild(0).localRotation = Quaternion.Slerp(holdPos.GetChild(0).localRotation, Quaternion.Euler(swayRot + inventory.Items[inventory.Index].properties.holdAngles), Time.deltaTime * swaySmoothRot);
        }
    }

    void DestroyChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}