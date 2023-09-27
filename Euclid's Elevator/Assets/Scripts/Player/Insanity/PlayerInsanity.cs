using DeathFloor.Enemy;
using DeathFloor.Player;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Insanity
{
    internal class PlayerInsanity : MonoBehaviour, IInsanityManager
    {
        [SerializeField, RequireInterface(typeof(IPlayer))] private Object _playerObject;
        [SerializeField] private float _maxInsanity;
        [SerializeField] private float _sanityDrainPerSec;
        [SerializeField] private string _visibilityRaycastProviderParentName;

        private bool _enabled;
        private float _insanity;
        private IVisibilityRaycastProvider _visibility;
        private IPlayer _player;

        private void Start()
        {
            _player = _playerObject as IPlayer;
            _visibility = GameObject.Find(_visibilityRaycastProviderParentName).GetComponent<IVisibilityRaycastProvider>();

            Enable();
        }

        void Update()
        {
            if (_enabled && (_visibility?.IsVisible() ?? false) &&
                _insanity < _maxInsanity)
            {
                _insanity += Time.deltaTime * _sanityDrainPerSec;
                if (_insanity >= _maxInsanity)
                {
                    _player?.Die();
                }
            }
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void ResetInsanity()
        {
            _insanity = 0;
        }

        public void ReduceInsanity(float percentage)
        {
            percentage = Mathf.Clamp01(percentage);
            _insanity -= _insanity * percentage;
            if (_insanity < 0) _insanity = 0;
        }
    }
}