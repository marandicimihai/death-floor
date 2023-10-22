using UnityEngine;
using UnityEngine.InputSystem;

namespace DeathFloor.Input
{
    [CreateAssetMenu(fileName = "New InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, PlayerInputActions.IGeneralActions, PlayerInputActions.IRealtimeActions, PlayerInputActions.IBoxActions
    {
        public delegate void InputDetected();
        public delegate void InventoryDetected(int index);
        public delegate void ScrollDetected(float value);

        public Vector2 Look => _inputActions.General.Look.ReadValue<Vector2>();

        public Vector2 Move => _inputActions.General.Movement.ReadValue<Vector2>();

        public ScrollDetected Scrolled;

        public bool Sneaking => _sneaking;

        public event InputDetected Dropped;
        public event InputDetected Used;
        public event InputDetected JournalToggled;
        public event InputDetected PageLeft;
        public event InputDetected PageRight;
        public event InputDetected PauseToggled;
        public event InputDetected BoxExited;

        public event InputDetected StartInteract;
        public event InputDetected EndInteract;

        public event InventoryDetected Inventory;

        private PlayerInputActions _inputActions;
        private bool _sneaking;

        private void OnEnable()
        {
            _inputActions = new PlayerInputActions();

            _inputActions.General.SetCallbacks(this);
            _inputActions.Realtime.SetCallbacks(this);
            _inputActions.Box.SetCallbacks(this);
            DefaultInput();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        #region Input Presets

        public void DefaultInput()
        {
            _inputActions.General.Enable();
            _inputActions.Realtime.Enable();
            _inputActions.Box.Disable();
        }

        public void BoxInput()
        {
            _inputActions.General.Disable();
            _inputActions.Realtime.Enable();
            _inputActions.Box.Enable();
        }

        public void DisableInput()
        {
            _inputActions.Disable();
        }

        public void PauseInput()
        {
            _inputActions.General.Disable();
            _inputActions.Realtime.Enable();
            _inputActions.Box.Disable();
        }

        #endregion

        #region Events

        public void OnDrop(InputAction.CallbackContext context)
        {
            if (Dropped != null &&
                context.phase == InputActionPhase.Performed)
            {
                Dropped.Invoke();
            }
        }

        public void OnExitBox(InputAction.CallbackContext context)
        {
            if (BoxExited != null &&
                context.phase == InputActionPhase.Performed)
            {
                BoxExited.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (StartInteract != null &&
                context.phase == InputActionPhase.Started)
            {
                StartInteract.Invoke();
            }
            else if (EndInteract != null &&
                context.phase == InputActionPhase.Canceled)
            {
                EndInteract.Invoke();
            }
        }

        public void OnInventory1(InputAction.CallbackContext context)
        {
            if (Inventory != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory.Invoke(0);
            }
        }

        public void OnInventory2(InputAction.CallbackContext context)
        {
            if (Inventory != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory.Invoke(1);
            }
        }

        public void OnInventory3(InputAction.CallbackContext context)
        {
            if (Inventory != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory.Invoke(2);
            }
        }

        public void OnInventory4(InputAction.CallbackContext context)
        {
            if (Inventory != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory.Invoke(3);
            }
        }

        public void OnJournal(InputAction.CallbackContext context)
        {
            if (JournalToggled != null &&
                context.phase == InputActionPhase.Performed)
            {
                JournalToggled.Invoke();
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {

        }

        public void OnMovement(InputAction.CallbackContext context)
        {

        }

        public void OnPageLeft(InputAction.CallbackContext context)
        {
            if (PageLeft != null &&
                context.phase == InputActionPhase.Performed)
            {
                PageLeft.Invoke();
            }
        }

        public void OnPageRight(InputAction.CallbackContext context)
        {
            if (PageRight != null &&
                context.phase == InputActionPhase.Performed)
            {
                PageRight.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (PauseToggled != null &&
                context.phase == InputActionPhase.Performed)
            {
                PauseToggled.Invoke();
            }
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            if (Scrolled != null &&
                context.phase == InputActionPhase.Performed)
            {
                Scrolled.Invoke(context.ReadValue<float>());
            }
        }

        public void OnSneak(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                _sneaking = true;
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                _sneaking = false;
            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (Used != null &&
                context.phase == InputActionPhase.Performed)
            {
                Used.Invoke();
            }
        }

        #endregion

        public string GetInteractionInputString()
        {
            return _inputActions.General.Interact.GetBindingDisplayString();
        }
    }
}