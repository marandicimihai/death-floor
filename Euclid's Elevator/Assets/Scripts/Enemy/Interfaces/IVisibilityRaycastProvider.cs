using DeathFloor.Utilities;

namespace DeathFloor.Enemy
{
    public interface IVisibilityRaycastProvider : IRaycastProvider
    {
        public bool IsVisible();
        public bool CanSeeCamera();
    }
}