using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Camera.Rotation
{
    internal class DefaultCameraRotationProvider : MonoBehaviour, ICameraRotationProvider
    {
        [Header("Settings")]
        [SerializeField, Range(1, 100)] private float _sensitivity = 25f;
        [SerializeField, RequireInterface(typeof(IUnityService))] private Object _unityService;

        private IUnityService _service;

        private void Start()
        {
            _service = _unityService as IUnityService;
        }

        public Vector2 CalculateRotation(Vector2 delta, Vector2 rotation)
        {
            delta *= _service.GetDeltaTime();

            rotation.x -= delta.y * _sensitivity;
            rotation.y += delta.x * _sensitivity;

            rotation.x = Mathf.Clamp(rotation.x, -90, 90);

            return rotation;
        }

        public Vector2 ResetAngle()
        {
            return Vector2.zero;
        }
    }
}