namespace DeathFloor.Inventory
{
    public interface IInventoryManager
    {
        public void PickUp(IItem item);
        public ItemProperties GetActiveItem();
        public void DecreaseDurability();
        public void ClearInventory();
    }
}