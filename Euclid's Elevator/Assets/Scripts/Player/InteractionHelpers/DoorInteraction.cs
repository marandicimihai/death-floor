using DeathFloor.Door;
using DeathFloor.HUD;
using DeathFloor.Inventory;
using DeathFloor.Utilities;
using System;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class DoorInteraction : MonoBehaviour, IInteractionHelper
    {
        public InteractionTag TargetInteractionTag => _targetTag;

        [SerializeField] private InteractionTag _targetTag;
        [SerializeField, RequireInterface(typeof(IInventoryManager))] private UnityEngine.Object _inventoryManager;
        [SerializeField, RequireInterface(typeof(IRaycastProvider))] private UnityEngine.Object _interactionRaycast;
        [SerializeField, RequireInterface(typeof(IActionInfo))] private UnityEngine.Object _actionInfo;

        private IActionInfo _info;
        private IRaycastProvider _interaction;
        private IInventoryManager _inventory;
        private float _startTime;
        private bool _isPicking;
        private bool _isLocking;
        private IDoor _current;

        private void Start()
        {
            _info = _actionInfo as IActionInfo;
            _interaction = _interactionRaycast as IRaycastProvider;
        }

        private void Update()
        {
            if (_isLocking)
            {
                try
                {
                    _info.SetSliderValue(this, (Time.time - _startTime) / _current.LockTime);
                }
                catch (DivideByZeroException dbze)
                {
                    Debug.Log("Can't divide by zero");
                    Debug.Log(dbze.Message);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }

                if (_current.Locked)
                {
                    _inventory?.DecreaseDurability();
                    _isLocking = false;
                    _info.StopSlider(this);
                    return;
                }

                //if changed active item or not looking at current door
                if (!_current.CheckKey(_inventory?.GetActiveItem()) ||
                    !_interaction.GetRaycast(out RaycastHit hit) || 
                    !hit.transform.TryGetComponent(out IInteractable door) ||
                    door.GetRoot().GetComponent<IDoor>() != _current)
                {
                    _isLocking = false;
                    _current?.InterruptLock();
                    _info.StopSlider(this);
                }
            }
            else if (_isPicking)
            {
                try
                {
                    _info.SetSliderValue(this, (Time.time - _startTime) / _current.LockpickTime);
                }
                catch (DivideByZeroException dbze)
                {
                    Debug.Log("Can't divide by zero");
                    Debug.Log(dbze.Message);
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }

                if (!_current.Locked)
                {
                    _isPicking = false;
                    _info.StopSlider(this);
                    return;
                }

                if (!_interaction.GetRaycast(out RaycastHit hit) ||
                    !hit.transform.TryGetComponent(out IInteractable door) ||
                    door.GetRoot().GetComponent<IDoor>() != _current)
                {
                    _isPicking = false;
                    _info.StopSlider(this);
                    _current?.InterruptLockpick();
                }
            }
        }

        public void InteractionExtension(GameObject rootObject)
        {
            _current = null;
            _isPicking = false;
            _isLocking = false;

            if (!rootObject.TryGetComponent(out IDoor door))
                return;

            _inventory ??= _inventoryManager as IInventoryManager;

            if (door.Locked)
            {
                bool successful = door.TryUnlock(_inventory?.GetActiveItem());

                if (successful)
                {
                    _inventory.DecreaseDurability();
                }
                else
                {
                    _isPicking = true;
                    _current = door;
                    _startTime = Time.time;
                    door.TryLockpick();
                    _info.StartSlider(this, SliderType.LockUnlocked);
                }
            }
            else
            {
                bool successful = door.TryLock(_inventory?.GetActiveItem());

                if (successful)
                {
                    _isLocking = true;
                    _current = door;
                    _startTime = Time.time;
                    _info.StartSlider(this, SliderType.LockLocked);
                }
            }
        }

        public void EndInteractionExtension(GameObject rootObject)
        {
            _current?.InterruptLock();
            _current?.InterruptLockpick();
            _info.StopSlider(this);
        }
    }
}