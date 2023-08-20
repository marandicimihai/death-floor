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
        [SerializeField] private Optional<MonoBehaviour> _movementProviderBehaviour;
        [SerializeField] private Optional<MonoBehaviour> _sneakProviderBehaviour;

        private Vector3 _velocity = Vector3.zero;
        private IMovementProvider _movementProvider;
        private ISneakProvider _sneakProvider;
        private IOptionalAssigner _optionalAssigner;


        private void Start()
        {
            _optionalAssigner ??= new OptionalAssigner(this);

            _movementProvider = _optionalAssigner.AssignUsingGetComponent<IMovementProvider>(_movementProviderBehaviour);
            _sneakProvider = _optionalAssigner.AssignUsingGetComponent<ISneakProvider>(_sneakProviderBehaviour);
        }

        public Vector3 GetMoveVector()
        {
            var movement = Vector3.zero;
            if (_inputReader.Sneaking)
            {
                if (_sneakProvider != null)
                {
                    movement = _sneakProvider.CalculateMovement(_inputReader.Move, ref _velocity);
                }
            }
            else
            {
                if (_movementProvider != null)
                {
                    movement = _movementProvider.CalculateMovement(_inputReader.Move, ref _velocity);
                }
            }
            return movement;
        }
    }
}