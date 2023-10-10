using DeathFloor.Camera;
using DeathFloor.Enemy;
using DeathFloor.Player;
using DeathFloor.Utilities;
using System.Collections;
using UnityEngine;

namespace DeathFloor.Insanity
{
    internal class PlayerInsanity : MonoBehaviour, IInsanityManager
    {
        [SerializeField, RequireInterface(typeof(IPlayer))] private Object _playerObject;
        [SerializeField, RequireInterface(typeof(IVFX))] private Object _vfxObject;
        [SerializeField, RequireInterface(typeof(ICameraAnimation))] private Object _cameraAnimationObject;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _cameraReference;
        [SerializeField] private float _maxInsanity;
        [SerializeField] private float _sanityDrainPerSec;
        [SerializeField] private string _visibilityRaycastProviderParentName;
        [SerializeField] private float _deathBlackScreenDelay;
        [SerializeField] private float _insanityEffectFadeTime;

        private bool _enabled;
        private float _insanity;
        private IVisibilityRaycastProvider _visibility;
        private ICameraAnimation _cameraAnimation;
        private IVFX _vfx;
        private IPlayer _player;

        private void Start()
        {
            _vfx = _vfxObject as IVFX;
            _player = _playerObject as IPlayer;
            _cameraAnimation = _cameraAnimationObject as ICameraAnimation;

            var target = GameObject.Find(_visibilityRaycastProviderParentName);

            if (target != null)
                _visibility = target.GetComponent<IVisibilityRaycastProvider>();

            Enable();
        }

        void Update()
        {
            if (_enabled && (_visibility?.IsVisible() ?? false) &&
                _insanity < _maxInsanity)
            {
                _insanity += Time.deltaTime * _sanityDrainPerSec;

                _vfx.InsanityEffect(true, _insanityEffectFadeTime);

                if (_insanity >= _maxInsanity && (!_player?.Dead ?? false))
                {
                    _player?.Die();
                    _cameraAnimation?.EnterAnimation(_cameraReference);
                    _vfx?.BlackScreen(true);

                    _animator.SetTrigger("Dead");
                }
            }
            else
            {
                _vfx.InsanityEffect(false, _insanityEffectFadeTime);
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