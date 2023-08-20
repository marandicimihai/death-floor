using UnityEngine;
using DeathFloor.Input;
using DeathFloor.Utilities;

namespace DeathFloor.Camera.Rotation
{
    public class CameraController : MonoBehaviour, IToggleable
    {
        [Header("Input")]
        [SerializeField] private InputReader _inputReader;

        [Header("Variables")]
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _playerCamera;

        private ICameraRotationProvider _rotationProvider;
        private bool _canLook;

        private void Start()
        {
            _rotationProvider = GetComponent<ICameraRotationProvider>();

            ResetAngle();
            Enable();
        }

        private void Update()
        {
            if (_rotationProvider != null &&
                _canLook)
            {
                Vector2 rotation = _rotationProvider.CalculateRotation(_inputReader.Look);

                _playerCamera.localEulerAngles = new Vector3(rotation.x, 0, 0);
                _player.localEulerAngles = new Vector3(0, rotation.y, 0);
            }
        }

        public void ResetAngle()
        {
            if (_rotationProvider == null) return;

            Vector2 rotation = _rotationProvider.ResetAngle();

            _playerCamera.localEulerAngles = new Vector3(rotation.x, 0, 0);
            _player.localEulerAngles = new Vector3(0, rotation.y, 0);
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