using DeathFloor.Inventory;

namespace DeathFloor.Door
{
    public interface IDoor
    {
        public bool TryUnlock(ItemProperties key);
    }
}