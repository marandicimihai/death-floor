using UnityEngine;

namespace DeathFloor.Inventory
{
    public interface ISwayHandler
    {
        public void PerformSway(GameObject itemRootObject, ItemProperties properties);
    }
}