using UnityEngine;

namespace DeathFloor.Movement
{
    public interface IGravityProvider
    {
        public Vector3 ComputeGravity();
    }
}