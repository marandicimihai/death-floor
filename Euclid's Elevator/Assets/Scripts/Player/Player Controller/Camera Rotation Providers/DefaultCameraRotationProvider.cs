using DeathFloor.UnityServices;
using UnityEngine;

namespace DeathFloor.Camera.Rotation
{
    public class DefaultCameraRotationProvider : MonoBehaviour, ICameraRotationProvider
    {
        [Header("Settings")]
        [SerializeField, Range(1, 100)] float _sensitivity = 25f;

        private IUnityService _service;
        private Vector2 _rotation;

        private void Start()
        {
            _service ??= new UnityService();
        }

        public Vector2 CalculateRotation(Vector2 delta)
        {
            delta *= _service.GetDeltaTime();

            _rotation.x -= delta.y * _sensitivity;
            _rotation.y += delta.x * _sensitivity;

            _rotation.x = Mathf.Clamp(_rotation.x, -90, 90);

            return _rotation;
        }

        public Vector2 ResetAngle()
        {
            _rotation = Vector2.zero;

            return _rotation;
        }
    }
}