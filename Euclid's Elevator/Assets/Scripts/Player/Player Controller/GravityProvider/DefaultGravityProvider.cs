using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class DefaultGravityProvider : MonoBehaviour, IGravityProvider
    {
        [Header("Gravity Properties")]
        [SerializeField] private float _gravityForce;
        [Header("Optional")]
        [SerializeField] private Optional<MonoBehaviour> _groundedProviderBehaviour;

        private IUnityService _service;
        private IOptionalAssigner _optionalAssigner;
        private IRaycastProvider _groundedProvider;
        private Vector3 gravity;

        private void Start()
        {
            _optionalAssigner ??= new OptionalAssigner(this);
            _service = new UnityService();

            _groundedProvider = _optionalAssigner.AssignUsingGetComponent<IRaycastProvider>(_groundedProviderBehaviour);
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