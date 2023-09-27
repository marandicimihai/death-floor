using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Camera
{
    public interface ICameraAnimation : IToggleable
    {
        public void EnterAnimation(Transform parent);
        public void TransitionToAnimation(Transform parent);
        public void ExitAnimation();
    }
}