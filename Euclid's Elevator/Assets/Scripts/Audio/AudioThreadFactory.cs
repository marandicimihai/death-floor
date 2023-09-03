using UnityEngine;

namespace DeathFloor.Audio
{
    internal class AudioThreadFactory : MonoBehaviour, IAudioThreadFactory
    {
        public IAudioThread CreateThread(GameObject parent)
        {
            var audioThread = parent.AddComponent<AudioThread>();
            return audioThread;
        }
    }
}