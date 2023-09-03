using System.Collections.Generic;
using UnityEngine;

namespace DeathFloor.Audio
{
    internal class AudioThread : MonoBehaviour, IAudioThread
    {
        private Queue<SoundProperties> _soundQueue;
        private AudioSource _currentSource;
        private bool _destroy;
        private float _timeElapsed;

        private void Start()
        {
            _soundQueue ??= new();
        }

        private void Update()
        {
            if (_soundQueue.Count > 0)
            {
                if (_currentSource == null)
                {
                    _currentSource = gameObject.AddComponent<AudioSource>();
                }
                else
                {
                    if (!_currentSource.isPlaying)
                    {
                        AssignValues(_soundQueue.Peek(), _currentSource);
                        _currentSource.Play();
                    }
                    else if (_timeElapsed >= _currentSource.clip.length)
                    {
                        Destroy(_currentSource);
                        _currentSource = null;
                        _soundQueue.Dequeue();
                        if (_soundQueue.Count == 0)
                        {
                            Destroy();
                        }
                        _timeElapsed = 0;
                    }
                    else
                    {
                        if (_soundQueue.Peek().PlayPaused)
                        {
                            _timeElapsed += Time.unscaledDeltaTime;
                        }
                        else
                        {
                            _timeElapsed += Time.deltaTime;
                        }
                    }
                }
            }
        }

        public IAudioThread AddToQueue(SoundProperties properties)
        {
            _soundQueue ??= new();
            _soundQueue.Enqueue(properties);
            return this;
        }

        public void SetVolume(float volume)
        {
            if (_currentSource != null)
            {
                _currentSource.volume = volume;
            }
        }

        public void Clear()
        {
            _soundQueue.Clear();
            Destroy();
        }

        private void Destroy()
        {
            if (_destroy)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        public void DestroyObjectWhenFinished()
        {
            _destroy = true;
        }

        private void AssignValues(SoundProperties properties, AudioSource source)
        {
            source.clip = properties.Clip;
            source.outputAudioMixerGroup = properties.Output;
            source.loop = properties.Loop;
            source.volume = properties.Volume;
            source.pitch = properties.Pitch;
            source.panStereo = properties.StereoPan;
            source.spatialBlend = properties.SpatialBlend;
            source.reverbZoneMix = properties.ReverbZoneMix;
        }
    }
}