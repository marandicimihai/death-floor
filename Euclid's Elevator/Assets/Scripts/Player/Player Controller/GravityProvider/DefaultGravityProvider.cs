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
        
        private IRaycastProvider _groundedProvider;
        private Vector3 gravity;

        private void Start()
        {
            _groundedProvider = _groundedRaycastProvider as IRaycastProvider;
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