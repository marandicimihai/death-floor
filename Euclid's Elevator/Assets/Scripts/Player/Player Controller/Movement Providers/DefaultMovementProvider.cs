using DeathFloor.UnityServices;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class DefaultMovementProvider : MonoBehaviour, IMovementProvider
    {
        [Header("Movement Properties")]
        [SerializeField] private float _acceleration = 0.2f;
        [SerializeField] private float _maxSpeed = 5f;

        private IUnityService _service;

        private void Start()
        {
            _service = new UnityService();
        }

        public Vector3 CalculateMovement(Vector2 input, ref Vector3 velocity)
        {
            Vector3 inputDirection = transform.right * input.x + transform.forward * input.y;
            Vector3 wishVel = _maxSpeed * inputDirection;
            Vector3 addVel = _acceleration * (wishVel - velocity).normalized;

            //Handle a full stop
            if (addVel.magnitude > velocity.magnitude && input == Vector2.zero)
            {
                velocity = Vector3.zero;
                return velocity;
            }

            velocity += addVel;

            //Cap speed to max value
            if (velocity.magnitude > wishVel.magnitude && input != Vector2.zero)
            {
                velocity = velocity.normalized * wishVel.magnitude;
            }

            return velocity * _service.GetDeltaTime();
        }
    }
}