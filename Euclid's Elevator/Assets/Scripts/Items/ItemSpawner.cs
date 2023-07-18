using UnityEngine;
using System;

public class ItemSpawner : MonoBehaviour
{
    public ItemProperties[] possibleItems;
    public bool hasItem;

    public bool Spawn(ItemProperties item, out Item spawned)
    {
        spawned = null;
        if (Array.Find(possibleItems, (ItemProperties current) => current.name == item.name) && !hasItem)
        {
            if (Instantiate(item.physicalObject, transform.position, Quaternion.identity).TryGetComponent<Item>(out Item itemc))
                itemc.uses = itemc.properties.uses;

            spawned = itemc;

            hasItem = true;
            return true;
        }
        return false;
    }

    public void ForceSpawn(ItemProperties item, out Item spawned)
    {
        spawned = null;
        if (Instantiate(item.physicalObject, transform.position, Quaternion.identity).TryGetComponent<Item>(out Item itemc))
        {
            itemc.uses = itemc.properties.uses;
            spawned = itemc;
        }
        hasItem = true;
    }
}