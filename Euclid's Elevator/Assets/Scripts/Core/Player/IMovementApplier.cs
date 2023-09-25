using UnityEngine;

namespace DeathFloor.Movement
{
    public interface IMovementApplier
    {
        public Vector3 GetMoveVector();
        public void BoostForTime(float time, float multiplier);
    }
}