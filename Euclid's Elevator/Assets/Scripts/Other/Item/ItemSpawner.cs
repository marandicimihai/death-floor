using UnityEngine;
using System;

public class ItemSpawner : MonoBehaviour
{
    public ItemObject[] possibleItems;
    public bool hasItem;

    public bool Spawn(ItemObject item)
    {
        if (Array.Find(possibleItems, (ItemObject current) => current.name == item.name))
        {
            if (Instantiate(item.prefab, transform.position, Quaternion.identity).TryGetComponent<Item>(out Item itemc))
                itemc.uses = itemc.itemObj.uses;

            hasItem = true;
            return true;
        }
        return false;
    }

    public void ForceSpawn(ItemObject item)
    {
        if (Instantiate(item.prefab, transform.position, Quaternion.identity).TryGetComponent<Item>(out Item itemc))
        {
            itemc.uses = itemc.itemObj.uses;
        }
        hasItem = true;
    }
}
