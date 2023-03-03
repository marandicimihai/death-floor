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
            Instantiate(item.prefab, transform.position, Quaternion.identity);
            hasItem = true;
            return true;
        }
        return false;
    }
}
