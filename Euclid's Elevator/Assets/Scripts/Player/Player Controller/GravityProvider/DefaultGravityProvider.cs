using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class DefaultGravityProvider : MonoBehaviour, IGravityProvider
    {
        [Header("Gravity Properties")]
        [SerializeField] private float _gravityForce = -9.81f;
        [Header("Other")]
        [SerializeField, RequireInterface(typeof(IRaycastProvider))] private Object _groundedRaycastProvider;
        [SerializeField, RequireInterface(typeof(IUnityService))] private Object _unityService;

        private IUnityService _service;
        private IRaycastProvider _groundedProvider;
        private Vector3 gravity;

        private void Start()
        {
            _service = _unityService as IUnityService;

            _groundedProvider = _groundedRaycastProvider as IRaycastProvider;
        }

        private void Update()
        {
            if (_groundedProvider != null &&
                _groundedProvider.GetRaycast())
            {
                gravity.y = -1f * _service.GetDeltaTime();
            }
            else
            {
                gravity.y += _gravityForce * _service.GetDeltaTime() * _service.GetDeltaTime();
            }
        }

        public Vector3 ComputeGravity()
        {
            return gravity;
        }
    }
}