using UnityEngine;
using UnityEngine.InputSystem;

namespace DeathFloor.Input
{
    [CreateAssetMenu(fileName = "New InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, PlayerInputActions.IGeneralActions, PlayerInputActions.IRealtimeActions, PlayerInputActions.IBoxActions
    {
        public delegate void InputDetected();
        public delegate void ScrollDetected(float value);

        public Vector2 Look { get => _inputActions.General.Look.ReadValue<Vector2>(); }
        public Vector2 Move { get => _inputActions.General.Movement.ReadValue<Vector2>(); }

        public ScrollDetected Scrolled;

        public event InputDetected Dropped;
        public event InputDetected Used;
        public event InputDetected JournalToggled;
        public event InputDetected PageLeft;
        public event InputDetected PageRight;
        public event InputDetected PauseToggled;
        public event InputDetected BoxExited;

        public event InputDetected Interacted;
        public event InputDetected SneakStarted;
        public event InputDetected SneakCanceled;

        public event InputDetected Inventory1;
        public event InputDetected Inventory2;
        public event InputDetected Inventory3;
        public event InputDetected Inventory4;

        private PlayerInputActions _inputActions;

        void OnEnable()
        {
            _inputActions = new PlayerInputActions();

            _inputActions.General.SetCallbacks(this);
            _inputActions.Realtime.SetCallbacks(this);
            _inputActions.Box.SetCallbacks(this);

            _inputActions.Enable();
            _inputActions.Box.Disable();
        }

        void OnDisable()
        {
            _inputActions.Disable();
        }

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
            if (Interacted != null &&
                context.phase == InputActionPhase.Started)
            {
                Interacted.Invoke();
            }
        }

        public void OnInventory1(InputAction.CallbackContext context)
        {
            if (Inventory1 != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory1.Invoke();
            }
        }

        public void OnInventory2(InputAction.CallbackContext context)
        {
            if (Inventory2 != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory2.Invoke();
            }
        }

        public void OnInventory3(InputAction.CallbackContext context)
        {
            if (Inventory3 != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory3.Invoke();
            }
        }

        public void OnInventory4(InputAction.CallbackContext context)
        {
            if (Inventory4 != null &&
                context.phase == InputActionPhase.Performed)
            {
                Inventory4.Invoke();
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
            if (SneakStarted != null &&
                context.phase == InputActionPhase.Started)
            {
                SneakStarted.Invoke();
            }

            if (SneakCanceled != null &&
                context.phase == InputActionPhase.Canceled)
            {
                SneakCanceled.Invoke();
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
    }
}