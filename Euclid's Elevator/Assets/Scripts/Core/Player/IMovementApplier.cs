using UnityEngine;

namespace DeathFloor.Movement
{
    public interface IMovementApplier
    {
        /// <summary>
        /// This method should be called on every frame (Update) for it to function properly.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetMoveVector();
        public void BoostForTime(float time, float multiplier);
    }
}