using DeathFloor.Utilities;

namespace DeathFloor.Inventory
{
    public interface IInventoryDisplayer : IToggleable
    {
        public void RefreshView(IItem[] items, int selectedIndex);
    }
}