using DeathFloor.Input;
using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    [RequireComponent(typeof(CharacterController))]
    internal class FirstPersonController : MonoBehaviour, IToggleable
    {
        [Header("Input Reader")]
        [SerializeField] private InputReader _inputReader;

        [Header("Optional")]
        [SerializeField] private Optional<MonoBehaviour> _movementProviderBehaviour;
        [SerializeField] private Optional<MonoBehaviour> _sneakProviderBehaviour;

        private Vector3 _velocity = Vector3.zero;
        private IMovementProvider _movementProvider;
        private ISneakProvider _sneakProvider;
        private CharacterController _controller;
        private bool _canMove;

        private void Start()
        {
            if (_movementProviderBehaviour.Enabled)
            {
                _movementProvider = _movementProviderBehaviour.Value as IMovementProvider;
            }
            else
            {
                _movementProvider = GetComponent<IMovementProvider>();
            }
            if (_sneakProviderBehaviour.Enabled)
            {
                _sneakProvider = _sneakProviderBehaviour.Value as ISneakProvider;
            }
            else
            {
                _sneakProvider = GetComponent<ISneakProvider>();
            }

            _controller = GetComponent<CharacterController>();

            Enable();
        }

        private void Update()
        {
            if (_canMove &&
                _controller != null)
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
                _controller.Move(movement);
            }
        }

        public void Disable()
        {
            _canMove = false;
            _controller.enabled = false;
        }

        public void Enable()
        {
            _canMove = true;
            _controller.enabled = true;
        }
    }
}