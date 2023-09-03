using DeathFloor.UnityServices;
using UnityEngine;

namespace DeathFloor.Audio
{
    public interface IAudioThreadFactory
    {
        public IAudioThread CreateThread(GameObject parent, IUnityService unityService);
    }
}