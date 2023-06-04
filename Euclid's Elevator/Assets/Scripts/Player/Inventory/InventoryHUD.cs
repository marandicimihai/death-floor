using UnityEngine;
using System;

[RequireComponent(typeof(Inventory))]
public class InventoryHUD : MonoBehaviour
{
    [Header("ItemModels")]
    [SerializeField] Transform holdPos;

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

    void DestroyChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
