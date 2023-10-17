using UnityEngine;

namespace DeathFloor.Movement
{
    public interface IMovementProvider
    {
        public Vector3 CalculateMovement(Vector2 input, ref Vector3 velocity);
    }
}