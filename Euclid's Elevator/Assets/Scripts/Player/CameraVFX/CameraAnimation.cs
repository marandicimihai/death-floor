using UnityEngine;

namespace DeathFloor.Camera
{
    internal class CameraAnimation : MonoBehaviour, ICameraAnimation
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private Transform _defaultCameraParent;

        [SerializeField] private float _transitionTime;

        private bool _inAnimation;
        private bool _enabled;

        private Vector3 _initialPosition;
        private Vector3 _finalPosition;

        private Quaternion _initialRotation;
        private Quaternion _finalRotation;

        private float t;

        private void Start()
        {
            Enable();
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void Enable()
        {
            _enabled = true;
        }

        private void Update()
        {
            if (t < 0)
            {
                t += Time.deltaTime / _transitionTime;
                _camera.position = Vector3.Lerp(_initialPosition, _finalPosition, t);
                _camera.rotation = Quaternion.Lerp(_initialRotation, _finalRotation, t);
            }
        }

        public void EnterAnimation(Transform parent)
        {
            if (_inAnimation || !_enabled) return;

            _inAnimation = true;

            _camera.SetParent(parent);

            _initialPosition = _camera.localPosition;
            _initialRotation = _camera.localRotation;

            _finalPosition = Vector3.zero;
            _finalRotation = Quaternion.identity;

            t = 0;
        }

        public void ExitAnimation()
        {
            if (!_inAnimation) return;

            _inAnimation = false;

            _camera.SetParent(_defaultCameraParent);

            _initialPosition = _camera.localPosition;
            _initialRotation = _camera.localRotation;

            _finalPosition = Vector3.zero;
            _finalRotation = Quaternion.identity;

            t = 0;
        }

        public void TransitionToAnimation(Transform parent)
        {
            if (!_enabled) return;

            _camera.SetParent(parent);

            _initialPosition = _camera.localPosition;
            _initialRotation = _camera.localRotation;

            _finalPosition = Vector3.zero;
            _finalRotation = Quaternion.identity;

            t = 0;
        }
    }
}