using DeathFloor.Interactions;
using DeathFloor.Inventory;
using UnityEngine;

namespace DeathFloor.Door
{
    public class Door : MonoBehaviour, IInteractable, IDoor
    {
        public InteractionTag Tag => _interactionTag;

        public bool IsInteractable => _isInteractable;

        public string InteractionPrompt => _prompt;

        [SerializeField] private InteractionTag _interactionTag;
        [SerializeField] private bool _isInteractable;
        [SerializeField] private string _lockedInteractionPrompt;
        [SerializeField] private string _stageLockedInteractionPrompt;

        [SerializeField] private ItemProperties _key;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private bool _locked;
        [SerializeField] private bool _stageLocked;

        private string _prompt;

        private void Start()
        {
            if (_stageLocked) _locked = true;

            _rigidbody.isKinematic = _locked;

            RecalculatePrompt();
        }

        private void OnValidate()
        {
            if (_stageLocked) _locked = true;
            
            _rigidbody.isKinematic = _locked;
        }

        private void RecalculatePrompt()
        {
            _prompt = string.Empty;

            if (_locked) _prompt = _lockedInteractionPrompt;

            if (_stageLocked) _prompt = _stageLockedInteractionPrompt;
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

            if (key == _key)
            {
                _locked = false;
                _rigidbody.isKinematic = false;
                RecalculatePrompt();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
