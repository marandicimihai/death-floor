using UnityEngine;

namespace DeathFloor.UnityServices
{
    public class UnityService : IUnityService
    {
        public float GetDeltaTime() => Time.deltaTime;
        public float GetUnscaledDeltaTime() => Time.unscaledDeltaTime;
        public int GetRandomInt(int minInclusive, int maxEsclusive) => Random.Range(minInclusive, maxEsclusive);
    }
}