using DeathFloor.UnityServices;
using UnityEngine;

namespace DeathFloor.Audio
{
    internal class AudioThreadFactory : MonoBehaviour, IAudioThreadFactory
    {
        public IAudioThread CreateThread(GameObject parent, IUnityService unityService)
        {
            var audioThread = parent.AddComponent<AudioThread>();
            audioThread.Initialize(unityService);
            return audioThread;
        }
    }
}