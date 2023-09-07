using UnityEngine;

namespace DeathFloor.Inventory
{
    internal interface ISwayHandler
    {
        public void PerformSway(GameObject itemRootObject, ItemProperties properties);
    }
}