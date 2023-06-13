using UnityEngine;
using System;

public class ItemSpawner : MonoBehaviour
{
    public ItemProperties[] possibleItems;
    public bool hasItem;

    public bool Spawn(ItemProperties item)
    {
        if (Array.Find(possibleItems, (ItemProperties current) => current.name == item.name))
        {
            if (Instantiate(item.physicalObject, transform.position, Quaternion.identity).TryGetComponent<Item>(out Item itemc))
                itemc.uses = itemc.properties.uses;

            hasItem = true;
            return true;
        }
        return false;
    }

    public void ForceSpawn(ItemProperties item)
    {
        if (Instantiate(item.physicalObject, transform.position, Quaternion.identity).TryGetComponent<Item>(out Item itemc))
        {
            itemc.uses = itemc.properties.uses;
        }
        hasItem = true;
    }
}