using DeathFloor.Utilities;

namespace DeathFloor.Inventory
{
    internal interface IInventoryDisplayer : IToggleable
    {
        public void RefreshView(CollectableItem[] items, int selectedIndex);
    }
}