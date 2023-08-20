using UnityEngine;

namespace DeathFloor.UnityServices
{
    public class UnityService : IUnityService
    {
        public float GetDeltaTime() => Time.deltaTime;
    }
}