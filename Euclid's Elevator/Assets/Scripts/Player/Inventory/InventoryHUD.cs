using UnityEngine;
using System;

[RequireComponent(typeof(Inventory))]
public class InventoryHUD : MonoBehaviour
{
    [Header("ItemModels")]
    [SerializeField] Transform holdPos;
    [SerializeField] float swayStep;
    [SerializeField] float swayMaxStep;
    [SerializeField] float swayRotStep;
    [SerializeField] float swayMaxRotStep;
    [SerializeField] float swaySmooth;
    [SerializeField] float swaySmoothRot;

    Inventory inventory;
    bool hideHUD;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        inventory.OnItemsChanged += (object caller, EventArgs args) => RefreshVisualModels();
    }

    private void Update()
    {
        if (hideHUD)
        {
            if (holdPos.childCount > 0)
            {
                DestroyChildren(holdPos);
            }
        }
        else
        {
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

            Vector3 swayRot = new Vector3(swayRotLook.y, swayRotLook.x, swayRotLook.x);

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
