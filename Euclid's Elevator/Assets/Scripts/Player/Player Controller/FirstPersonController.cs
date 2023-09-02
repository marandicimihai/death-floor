using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class FirstPersonController : MonoBehaviour, IToggleable
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField, RequireInterface(typeof(IMovementApplier))] private Object _movementApplier;

        private IMovementApplier _movementApplierInterface;
        private bool _canMove;

        private void Start()
        {
            _movementApplierInterface = _movementApplier as IMovementApplier;
            Enable();
        }

        private void Update()
        {
            if (_canMove &&
                _controller != null &&
                _movementApplierInterface != null)
            {
                _controller.Move(_movementApplierInterface.GetMoveVector());
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