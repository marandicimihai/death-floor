using UnityEngine.UI;
using UnityEngine;
using System;

[RequireComponent(typeof(Inventory))]
public class InventoryHUD : MonoBehaviour
{
    [System.NonSerialized] public bool hideHUD;
    [Header("ItemModels")]
    public Transform holdPos;
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
        {
            SetAllInvisible();
            return;
        }

        SetActiveItemVisible();
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

            inventory.Items[inventory.Index].transform.localPosition = Vector3.Lerp(inventory.Items[inventory.Index].transform.localPosition, swayPos + inventory.Items[inventory.Index].properties.offset, Time.deltaTime * swaySmooth);
            inventory.Items[inventory.Index].transform.localRotation = Quaternion.Slerp(inventory.Items[inventory.Index].transform.localRotation, Quaternion.Euler(swayRot + inventory.Items[inventory.Index].properties.holdAngles), Time.deltaTime * swaySmoothRot);
        }
    }

    void SetActiveItemVisible()
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if (inventory.Items[i] != null && i == inventory.Index)
            {
                inventory.Items[i].SetVisible(true);
            }
            else if (inventory.Items[i] != null)
            {
                inventory.Items[i].SetVisible(false);
            }
            if (inventory.Items[i] != null)
            {
                inventory.Items[i].transform.localPosition = inventory.Items[i].properties.offset;
                inventory.Items[i].transform.localEulerAngles = inventory.Items[i].properties.holdAngles;
            }
        }
    }
    void SetAllInvisible()
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            inventory.Items[i].SetVisible(false);
        }
    }
}