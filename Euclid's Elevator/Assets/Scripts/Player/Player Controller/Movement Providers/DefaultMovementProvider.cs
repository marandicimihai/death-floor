using DeathFloor.UnityServices;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class DefaultMovementProvider : MonoBehaviour, IMovementProvider
    {
        [Header("Movement Properties")]
        [SerializeField] private float _acceleration;
        [SerializeField] private float _maxSpeed;

        private IUnityService _service;
        private Vector3 _velocity;

        private void Start()
        {
            _service ??= new UnityService();
        }

        public Vector3 CalculateMovement(Vector2 input)
        {
            Vector3 inputDirection = transform.right * input.x + transform.forward * input.y;
            Vector3 wishVel = _maxSpeed * inputDirection;
            Vector3 addVel = _acceleration * (wishVel - _velocity).normalized;

            //Handle a full stop
            if (addVel.magnitude > _velocity.magnitude && input == Vector2.zero)
            {
                _velocity = Vector3.zero;
                return _velocity;
            }

            _velocity += addVel;

            //Cap speed to max value
            if (_velocity.magnitude > wishVel.magnitude && input != Vector2.zero)
            {
                _velocity = _velocity.normalized * wishVel.magnitude;
            }

            return _velocity * _service.GetDeltaTime();
        }
    }
}