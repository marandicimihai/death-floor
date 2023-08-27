using UnityEngine;

namespace DeathFloor.Movement
{
    internal interface IGravityProvider
    {
        public Vector3 ComputeGravity();
    }
}