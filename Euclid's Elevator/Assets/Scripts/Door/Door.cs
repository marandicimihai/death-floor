using DeathFloor.Interactions;
using DeathFloor.Inventory;
using System.Collections;
using UnityEngine;

namespace DeathFloor.Door
{
    public class Door : MonoBehaviour, IInteractable, IDoor
    {
        public InteractionTag Tag => _interactionTag;
        public bool IsInteractable => _isInteractable;
        public string InteractionPrompt => RecalculatePrompt();
        public bool Locked => _stageLocked || _locked;
        public float LockpickTime => _lockpickTime;
        public float LockTime => _lockTime;

        [SerializeField] private InteractionTag _interactionTag;
        [SerializeField] private bool _isInteractable;
        [SerializeField] private string _lockedInteractionPrompt;
        [SerializeField] private string _stageLockedInteractionPrompt;
        [SerializeField] private float _lockpickTime;
        [SerializeField] private float _lockTime;

        [SerializeField] private ItemProperties _key;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private bool _locked;
        [SerializeField] private bool _stageLocked;

        private bool _lockInterrupted;
        private bool _pickInterrupted;

        private void Start()
        {
            if (_stageLocked) _locked = true;

            _rigidbody.isKinematic = _locked;

        }

        private void OnValidate()
        {
            if (_stageLocked) _locked = true;

            _rigidbody.isKinematic = _locked;
        }

        private string RecalculatePrompt()
        {
            if (_locked) return _lockedInteractionPrompt;

            if (_stageLocked) return _stageLockedInteractionPrompt;

            return string.Empty;
        }

        public GameObject GetRoot()
        {
            return gameObject;
        }

        public void Interact()
        {

        }

        public bool TryUnlock(ItemProperties key)
        {
            if (_stageLocked || !_locked)
                return false;

            if (CheckKey(key))
            {
                _locked = false;
                _rigidbody.isKinematic = false;
                InterruptLock();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckKey(ItemProperties key)
        {
            if (key == _key)
                return true;

            return false;
        }

        public bool TryLock(ItemProperties key)
        {
            if (_stageLocked || _locked)
                return false;

            if (CheckKey(key))
            {
                _lockInterrupted = false;
                CancelInvoke(nameof(Lock));
                Invoke(nameof(Lock), _lockTime);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void InterruptLock()
        {
            _lockInterrupted = true;
        }

        private void Lock()
        {
            if (_lockInterrupted)
            {
                _lockInterrupted = false;
                return;
            }

            _locked = true;
            _rigidbody.isKinematic = true;
            _rigidbody.transform.localRotation = Quaternion.identity;
        }

        public void TryLockpick()
        {
            if (_stageLocked || !_locked)
                return;

            _pickInterrupted = false;
            CancelInvoke(nameof(PickLock));
            Invoke(nameof(PickLock), _lockpickTime);
        }

        public void InterruptLockpick()
        {
            _pickInterrupted = true;
        }

        private void PickLock()
        {
            if (_pickInterrupted)
            {
                _pickInterrupted = false;
                return;
            }

            _locked = false;
            _rigidbody.isKinematic = false;
        }
    }
}
