using DeathFloor.Input;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    [RequireComponent(typeof(CharacterController))]
    internal class MovementApplier : MonoBehaviour, IMovementApplier
    {
        [Header("Input Reader")]
        [SerializeField] private InputReader _inputReader;

        [Header("Other")]
        [SerializeField] private LayerMask _walls;
        [SerializeField] private float _maxSlopeAngle;
        [SerializeField, RequireInterface(typeof(IRaycastProvider))] private Object _slopeRaycastProvider;

        [Header("Optional")]
        [SerializeField, RequireInterface(typeof(IMovementProvider))] private Object _movementProvider;
        [SerializeField, RequireInterface(typeof(IMovementProvider))] private Object _sneakMovementProvider;
        [SerializeField, RequireInterface(typeof(IGravityProvider))] private Object _gravityProvider;

        private Vector3 _velocity = Vector3.zero;

        private float _movementMutltiplier = 1;
        private IMovementProvider _movementProviderInterface;
        private IMovementProvider _sneakMovementProviderInterface;
        private IGravityProvider _gravityProviderInterface;
        private IRaycastProvider _slopeProvider;

        private void Start()
        {
            _movementProviderInterface = _movementProvider as IMovementProvider;
            _sneakMovementProviderInterface = _sneakMovementProvider as IMovementProvider;
            _gravityProviderInterface = _gravityProvider as IGravityProvider;
            _slopeProvider = _slopeRaycastProvider as IRaycastProvider;
        }

        public Vector3 GetMoveVector()
        {
            var movement = Vector3.zero;
            if (_inputReader.Sneaking)
            {
                if (_sneakMovementProviderInterface != null)
                {
                    movement = _sneakMovementProviderInterface.CalculateMovement(_inputReader.Move, ref _velocity);
                }
            }
            else
            {
                if (_movementProviderInterface != null)
                {
                    movement = _movementProviderInterface.CalculateMovement(_inputReader.Move, ref _velocity);
                }
            }
            movement *= _movementMutltiplier;
            if (_gravityProviderInterface != null)
            {
                movement += _gravityProviderInterface.ComputeGravity();
            }
            return movement;
        }

        public void BoostForTime(float time, float multiplier)
        {
            _movementMutltiplier = multiplier;
            CancelInvoke(nameof(ResetBoost));
            Invoke(nameof(ResetBoost), time);
        }

        private void ResetBoost()
        {
            _movementMutltiplier = 1;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (((1 << hit.collider.gameObject.layer) & _walls.value) > 0)
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                if (angle >= _maxSlopeAngle)
                {
                    _velocity = Vector3.ProjectOnPlane(_velocity, hit.normal);
                }
            }

            if (_slopeProvider.GetRaycast(out RaycastHit slopeHit))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

                if (angle < _maxSlopeAngle)
                {
                    if (angle != 0)
                    {
                        _velocity = Vector3.ProjectOnPlane(_velocity, slopeHit.normal);
                        return;
                    }
                    else
                    {
                        _velocity = Vector3.ProjectOnPlane(_velocity, Vector3.up);
                    }
                }
            }
        }
    }
}