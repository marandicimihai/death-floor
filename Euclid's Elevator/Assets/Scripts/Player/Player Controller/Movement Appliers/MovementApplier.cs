using DeathFloor.Input;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class MovementApplier : MonoBehaviour, IMovementApplier
    {
        [Header("Input Reader")]
        [SerializeField] private InputReader _inputReader;

        [Header("Optional")]
        [SerializeField, RequireInterface(typeof(IMovementProvider))] private Object _movementProvider;
        [SerializeField, RequireInterface(typeof(IMovementProvider))] private Object _sneakMovementProvider;
        [SerializeField, RequireInterface(typeof(IGravityProvider))] private Object _gravityProvider;

        private Vector3 _velocity = Vector3.zero;

        private float _movementMutltiplier = 1;
        private IMovementProvider _movementProviderInterface;
        private IMovementProvider _sneakMovementProviderInterface;
        private IGravityProvider _gravityProviderInterface;


        private void Start()
        {
            _movementProviderInterface = _movementProvider as IMovementProvider;
            _sneakMovementProviderInterface = _sneakMovementProvider as IMovementProvider;
            _gravityProviderInterface = _gravityProvider as IGravityProvider;
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
    }
}