using DeathFloor.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DeathFloor.Audio
{
    public interface IAudioPlayer : IToggleable
    {
        public IAudioThread PlayAtAudioPlayer(SoundProperties properties);
        public IAudioThread PlayAtPosition(SoundProperties properties, Vector3 position);
        public IAudioThread PlayAtTransform(SoundProperties properties, Transform parent);
        public IAudioThread PlayRandomAtAudioPlayer(IEnumerable<SoundProperties> properties);
        public IAudioThread PlayRandomAtPosition(IEnumerable<SoundProperties> properties, Vector3 position);
        public IAudioThread PlayRandomAtTransform(IEnumerable<SoundProperties> properties, Transform parent);
    }
}