namespace DeathFloor.Inventory
{
    internal interface IInventoryManager
    {
        public void PickUp(CollectableItem item);
        public ItemProperties GetActiveItem();
        public void DecreaseDurability();
        public void ClearInventory();
    }
}