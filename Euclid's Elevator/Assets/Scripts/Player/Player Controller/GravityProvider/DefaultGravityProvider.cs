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

        private IOptionalAssigner _optionalAssigner;
        private IRaycastProvider _groundedProvider;
        private Vector3 gravity;

        private void Start()
        {
            _optionalAssigner ??= new OptionalAssigner(this);

            _groundedProvider = _optionalAssigner.AssignUsingGetComponent<IRaycastProvider>(_groundedProviderBehaviour);
        }

        private void Update()
        {
            if (_groundedProvider != null &&
                _groundedProvider.GetRaycast())
            {
                gravity.y = -1f * Time.deltaTime;
            }
            else
            {
                gravity.y += _gravityForce * Time.deltaTime * Time.deltaTime;
            }
        }

        public Vector3 ComputeGravity()
        {
            return gravity;
        }
    }
}