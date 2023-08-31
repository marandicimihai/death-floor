using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DeathFloor.Audio
{
    internal class AudioPlayer : MonoBehaviour, IAudioPlayer, IToggleable
    {
        private IUnityService _service;
        private bool _canPlay;

        private void Start()
        {
            _service = new UnityService();

            Enable();
        }

        public void Disable()
        {
            _canPlay = false;
        }

        public void Enable()
        {
            _canPlay = true;
        }

        public IAudioThread PlayAtAudioPlayer(SoundProperties properties)
        {
            if (_canPlay)
            {
                IAudioThread thread = gameObject.AddComponent<AudioThread>();
                thread.AddToQueue(properties);
                return thread;
            }
            else
            {
                return null;
            }
        }

        public IAudioThread PlayAtPosition(SoundProperties properties, Vector3 position)
        {
            if (_canPlay)
            {
                GameObject obj = new();
                obj.transform.position = position;
                IAudioThread thread = obj.AddComponent<AudioThread>();
                thread.AddToQueue(properties);
                thread.DestroyObjectWhenFinished();
                return thread;
            }
            else
            {
                return null;
            }
        }

        public IAudioThread PlayAtTransform(SoundProperties properties, Transform parent)
        {
            if (_canPlay)
            {
                IAudioThread thread = parent.gameObject.AddComponent<AudioThread>();
                thread.AddToQueue(properties);
                return thread;
            }
            else
            {
                return null;
            }
        }

        public IAudioThread PlayRandomAtAudioPlayer(IEnumerable<SoundProperties> properties)
        {
            if (_canPlay)
            {
                IAudioThread thread = gameObject.AddComponent<AudioThread>();
                thread.AddToQueue(properties.ElementAt(_service.GetRandomInt(0, properties.Count())));
                return thread;
            }
            else
            {
                return null;
            }
        }

        public IAudioThread PlayRandomAtPosition(IEnumerable<SoundProperties> properties, Vector3 position)
        {
            if (_canPlay)
            {
                GameObject obj = new();
                obj.transform.position = position;
                IAudioThread thread = obj.AddComponent<AudioThread>();
                thread.AddToQueue(properties.ElementAt(_service.GetRandomInt(0, properties.Count())));
                thread.DestroyObjectWhenFinished();
                return thread;
            }
            else
            {
                return null;
            }
        }

        public IAudioThread PlayRandomAtTransform(IEnumerable<SoundProperties> properties, Transform parent)
        {
            if (_canPlay)
            {
                IAudioThread thread = parent.gameObject.AddComponent<AudioThread>();
                thread.AddToQueue(properties.ElementAt(_service.GetRandomInt(0, properties.Count())));
                return thread;
            }
            else
            {
                return null;
            }
        }
    }
}