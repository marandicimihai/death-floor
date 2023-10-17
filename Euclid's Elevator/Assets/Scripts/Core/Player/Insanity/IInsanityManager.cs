using DeathFloor.Utilities;

namespace DeathFloor.Insanity
{
    public interface IInsanityManager : IToggleable
    {
        public void ResetInsanity();
        public void ReduceInsanity(float percentage);
    }
}