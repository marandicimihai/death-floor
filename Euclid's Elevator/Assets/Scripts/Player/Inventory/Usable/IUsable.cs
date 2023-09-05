namespace DeathFloor.Inventory
{
    internal interface IUsable
    {
        public UseTag UseTag { get; }
        public void OnUse();
    }
}