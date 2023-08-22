using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    public enum RayType
    {
        Transform = 0,
        CharacterController = 1
    }

    public class GroundedRaycastProvider : MonoBehaviour, IRaycastProvider
    {
        [SerializeField] CharacterController _controller;
        [SerializeField] float _extraLength;

        [SerializeField] LayerMask _layerMask;

        public bool GetRaycast()
        {
            return Physics.Raycast(_controller.transform.position + _controller.center,
                                   Vector3.down,
                                   _controller.height / 2 + _extraLength,
                                   _layerMask);

        }

        public RaycastHit GetRaycastHit()
        {
            Physics.Raycast(_controller.transform.position + _controller.center,
                            Vector3.down,
                            out RaycastHit hitInfo,
                            _controller.height / 2 + _extraLength,
                            _layerMask);
            return hitInfo;
        }
    }
}