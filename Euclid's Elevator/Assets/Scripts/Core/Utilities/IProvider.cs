namespace DeathFloor.Utilities
{
    public interface IProvider<T>
    {
        public T Get();
    }
}