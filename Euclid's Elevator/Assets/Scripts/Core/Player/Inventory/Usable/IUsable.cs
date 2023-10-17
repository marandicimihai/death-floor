namespace DeathFloor.Inventory
{
    public interface IUsable
    {
        public UseTag UseTag { get; }
        public void OnUse();
    }
}