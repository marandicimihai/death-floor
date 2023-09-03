using UnityEngine;
using DeathFloor.Input;
using DeathFloor.Utilities;

namespace DeathFloor.Camera.Rotation
{
    internal class CameraController : MonoBehaviour, IToggleable
    {
        [Header("Input")]
        [SerializeField] private InputReader _inputReader;

        [Header("Variables")]
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _playerCamera;
        [SerializeField, RequireInterface(typeof(ICameraRotationProvider))] private Object _rotationProvider;

        private ICameraRotationProvider _rotationProviderInterface;
        private Vector2 _rotation;
        private bool _canLook;

        private void Start()
        {
            _rotationProviderInterface = _rotationProvider as ICameraRotationProvider;

            Enable();
        }

        private void Update()
        {
            if (_canLook)
            {
                _rotation = _rotationProviderInterface?.CalculateRotation(_inputReader.Look, _rotation) ?? _rotation;

                _playerCamera.localEulerAngles = new Vector3(_rotation.x, 0, 0);
                _player.localEulerAngles = new Vector3(0, _rotation.y, 0);
            }
        }

        public void SyncRotation()
        {
            _rotation.x = _playerCamera.localEulerAngles.x;
            _rotation.y = _player.localEulerAngles.y;
        }

        public void ResetAngle()
        {
            if (_rotationProviderInterface == null) return;

            _rotation = _rotationProviderInterface.ResetAngle();

            _playerCamera.localEulerAngles = new Vector3(_rotation.x, 0, 0);
            _player.localEulerAngles = new Vector3(0, _rotation.y, 0);
        }

        public void Disable()
        {
            _canLook = false;
        }

        public void Enable()
        {
            _canLook = true;
        }
    }
}