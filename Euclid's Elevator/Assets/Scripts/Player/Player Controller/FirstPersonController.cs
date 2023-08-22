using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class FirstPersonController : MonoBehaviour, IToggleable
    {
        [SerializeField] private Optional<CharacterController> _characterController;
        [SerializeField] private Optional<MonoBehaviour> _movementApplierBehaviour;

        private CharacterController _controller;
        private IMovementApplier _movementApplier;
        private IOptionalAssigner _optionalAssigner;
        private bool _canMove;

        private void Start()
        {
            _optionalAssigner ??= new OptionalAssigner(this);

            _movementApplier = _optionalAssigner.AssignUsingGetComponent<IMovementApplier>(_movementApplierBehaviour);
            _controller = _optionalAssigner.AssignUsingGetComponent(_characterController);

            Enable();
        }

        private void Update()
        {
            if (_canMove &&
                _controller != null &&
                _movementApplier != null)
            {
                _controller.Move(_movementApplier.GetMoveVector());
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