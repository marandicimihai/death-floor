using UnityEngine;
using System;

[RequireComponent(typeof(Inventory))]
public class InventoryHUD : MonoBehaviour
{
    [Header("ItemModels")]
    [SerializeField] Transform holdPos;

    Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        inventory.OnItemsChanged += (object caller, EventArgs args) => RefreshVisualModels();
    }

    void RefreshVisualModels()
    {
        if (holdPos.childCount > 0)
        {
            if (inventory.Items[inventory.Index] != null && holdPos.GetComponentInChildren<Item>() &&
                holdPos.GetComponentInChildren<Item>().properties.name != inventory.Items[inventory.Index].properties.name)
            {
                Destroy(holdPos.GetChild(0).gameObject);
                Item item = inventory.Items[inventory.Index];
                GameObject model = Instantiate(item.properties.inHandObject, holdPos);
                model.transform.localPosition = item.properties.offset;
                model.transform.localEulerAngles = item.properties.holdAngles;
            }
            else if (inventory.Items[inventory.Index] == null)
            {
                Destroy(holdPos.GetChild(0).gameObject);
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
}
