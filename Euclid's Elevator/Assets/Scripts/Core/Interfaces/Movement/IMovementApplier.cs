using UnityEngine;

namespace DeathFloor.Movement
{
    public interface IMovementApplier
    {
        public Vector3 GetMoveVector();
    }
}