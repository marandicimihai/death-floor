using UnityEngine;

namespace DeathFloor.Inventory
{
    public interface ISwayHandler
    {
        /// <summary>
        /// This method should get called on every frame.
        /// </summary>
        /// <param name="itemRootObject"></param>
        /// <param name="properties"></param>
        public void PerformSway(GameObject itemRootObject, ItemProperties properties);
    }
}