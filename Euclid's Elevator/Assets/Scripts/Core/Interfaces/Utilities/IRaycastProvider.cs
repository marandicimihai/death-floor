using UnityEngine;

namespace DeathFloor.Utilities
{
    public interface IRaycastProvider
    {
        public RaycastHit GetRaycastHit();
        public bool GetRaycast();
        public bool GetRaycast(out RaycastHit hitInfo);
    }
}