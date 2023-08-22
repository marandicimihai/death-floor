using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    public class DefaultGravityProvider : MonoBehaviour, IGravityProvider
    {
        [Header("Gravity Properties")]
        [SerializeField] private float _gravityForce;
        [Header("Optional")]
        [SerializeField] private Optional<MonoBehaviour> _groundedProviderBehaviour;

        private IOptionalAssigner _optionalAssigner;
        private IRaycastProvider _groundedProvider;
        private IUnityService _service;
        private Vector3 gravity;

        private void Start()
        {
            _service ??= new UnityService();
            _optionalAssigner ??= new OptionalAssigner(this);

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