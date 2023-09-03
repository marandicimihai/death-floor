using DeathFloor.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DeathFloor.Audio
{
    internal class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        [SerializeField, RequireInterface(typeof(IAudioThreadFactory))] private Object _audioThreadFactory;
        
        private IAudioThreadFactory _factory;
        private bool _canPlay;

        private void Start()
        {
            _factory = _audioThreadFactory as IAudioThreadFactory;

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
                IAudioThread thread = _factory.CreateThread(gameObject);
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
                IAudioThread thread = _factory.CreateThread(obj);
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
                IAudioThread thread = _factory.CreateThread(parent.gameObject);
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
                IAudioThread thread = _factory.CreateThread(gameObject);
                thread.AddToQueue(properties.ElementAt(Random.Range(0, properties.Count())));
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
                IAudioThread thread = _factory.CreateThread(obj);
                thread.AddToQueue(properties.ElementAt(Random.Range(0, properties.Count())));
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
                IAudioThread thread = _factory.CreateThread(parent.gameObject);
                thread.AddToQueue(properties.ElementAt(Random.Range(0, properties.Count())));
                return thread;
            }
            else
            {
                return null;
            }
        }
    }
}