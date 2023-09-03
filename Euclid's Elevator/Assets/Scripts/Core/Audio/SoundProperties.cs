using UnityEngine;
using UnityEngine.Audio;

namespace DeathFloor.Audio
{
    [CreateAssetMenu(fileName = "New Sound", menuName = "Sound")]
    public class SoundProperties : ScriptableObject
    {
        public AudioClip Clip { get => _clip; }
        public AudioMixerGroup Output { get => _output; }
        public bool Loop { get => _loop; }
        public bool PlayPaused { get => _playPaused; }
        public float Volume { get => _volume; }
        public float Pitch { get => _pitch; }
        public float StereoPan { get => _stereoPan; }
        public float SpatialBlend {  get => _spatialBlend; }
        public float ReverbZoneMix { get => _reverbZoneMix; }

        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioMixerGroup _output;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _playPaused;
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
        [SerializeField, Range(-3f, 3f)] private float _pitch = 1f;
        [SerializeField, Range(-1f, 1f)] private float _stereoPan;
        [SerializeField, Range(0f, 1f)] private float _spatialBlend;
        [SerializeField, Range(0f, 1.1f)] private float _reverbZoneMix = 1f;
    }
}