namespace DeathFloor.Player
{
    public interface IPlayer
    {
        public bool Dead { get; }
        public void Spawn();
        public void Die();
    }
}