namespace DeathFloor.Inventory
{
    internal interface IPickupHandler
    {
        /// <summary>
        /// Returns the item to add to inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public CollectableItem PickUp(CollectableItem item);
    }
}