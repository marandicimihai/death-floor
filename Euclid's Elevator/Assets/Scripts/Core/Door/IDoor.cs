using DeathFloor.Inventory;

namespace DeathFloor.Door
{
    public interface IDoor
    {
        public bool Locked { get; }
        public float LockpickTime { get; }
        public float LockTime { get; }

        public bool TryUnlock(ItemProperties key);
        public void TryLockpick();
        public void InterruptLockpick();
        public bool CheckKey(ItemProperties key);
        public bool TryLock(ItemProperties key);
        public void InterruptLock();
    }
}