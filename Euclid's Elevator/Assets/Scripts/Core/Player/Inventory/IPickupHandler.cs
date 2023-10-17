namespace DeathFloor.Inventory
{
    public interface IPickupHandler
    {
        /// <summary>
        /// Returns the item to add to inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IItem PickUp(IItem item);
    }
}