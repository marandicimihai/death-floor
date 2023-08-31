using UnityEngine;
using UnityEngine.Audio;

namespace DeathFloor.Audio
{
    [CreateAssetMenu(fileName = "New Sound", menuName = "Sound")]
    public class SoundProperties : ScriptableObject
    {
        public AudioClip Clip;
        public AudioMixerGroup Output;
        public bool Loop;
        public bool PlayPaused;
        [Range(0f, 1f)] public float Volume = 1f;
        [Range(-3f, 3f)] public float Pitch = 1f;
        [Range(-1f, 1f)] public float StereoPan;
        [Range(0f, 1f)] public float SpatialBlend;
        [Range(0f, 1.1f)] public float ReverbZoneMix = 1f;
    }
}