using DeathFloor.Interactions;
using System.Collections;
using UnityEngine;

namespace DeathFloor.Elevator
{
    [SelectionBase]
    internal class Elevator : MonoBehaviour, IElevator, IInteractable
    {
        public InteractionTag Tag => _tag;
        public bool IsInteractable => _isInteractable;
        public string InteractionPrompt => _interactionPrompt;

        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _doorCollider;
        [SerializeField] private float _rideTime = 5f;

        [SerializeField] private InteractionTag _tag;
        [SerializeField] private bool _isInteractable;
        [SerializeField] private string _interactionPrompt;

        private void Start()
        {
            Ride(true);
        }

        public void Ride(bool instaClose)
        {
            StartCoroutine(ElevatorRideCoroutine(instaClose));
        }

        IEnumerator ElevatorRideCoroutine(bool instaClose)
        {
            CloseDoors(instaClose);

            yield return new WaitForSeconds(_rideTime);

            OpenDoors();
        }

        private void OpenDoors(bool instant = false)
        {
            _animator.SetBool("Open", true);

            if (instant)
            {
                _animator.SetTrigger("InstaOpen");
            }

            _doorCollider.SetActive(false);
        }

        private void CloseDoors(bool instant)
        {
            _animator.SetBool("Open", false);
            
            if (instant)
            {
                _animator.SetTrigger("InstaClose");
            }

            _doorCollider.SetActive(true);
        }

        public void Interact()
        {
        }

        public GameObject GetRoot()
        {
            return gameObject;
        }
    }
}