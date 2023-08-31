namespace DeathFloor.UnityServices
{
    public interface IUnityService
    {
        public float GetDeltaTime();
        public float GetUnscaledDeltaTime();
        public int GetRandomInt(int minInclusive, int maxExclusive);
    }
}