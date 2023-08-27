using UnityEngine;

namespace DeathFloor.Movement
{
    internal interface IMovementApplier
    {
        public Vector3 GetMoveVector();
    }
}