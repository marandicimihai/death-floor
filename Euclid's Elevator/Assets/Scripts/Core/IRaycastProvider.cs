using UnityEngine;

namespace DeathFloor.Utilities
{
    public interface IRaycastProvider
    {
        public RaycastHit GetRaycastHit();
        public bool GetRaycast();
    }
}