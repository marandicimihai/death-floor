using DeathFloor.Input;
using DeathFloor.UnityServices;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour, IToggleable
    {
        [Header("Input Reader")]
        [SerializeField] private InputReader _inputReader;

        private IMovementProvider _movementProvider;
        private CharacterController _controller;
        private bool _canMove;

        private void Start()
        {
            _movementProvider = GetComponent<IMovementProvider>();
            _controller = GetComponent<CharacterController>();

            Enable();
        }

        private void Update()
        {
            if (_movementProvider != null &&
                _controller != null &&
                _canMove)
            {
                Vector3 movement = _movementProvider.CalculateMovement(_inputReader.Move);
                _controller.Move(movement);
            }
        }

        public void Disable()
        {
            _canMove = false;
        }

        public void Enable()
        {
            _canMove = true;
        }
    }
}