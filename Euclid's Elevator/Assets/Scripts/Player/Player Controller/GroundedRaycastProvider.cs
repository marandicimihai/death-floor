using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Movement
{
    internal class GroundedRaycastProvider : MonoBehaviour, IRaycastProvider
    {
        [SerializeField] CharacterController _controller;
        [SerializeField] float _extraLength;

        [SerializeField] LayerMask _layerMask;

        public RaycastHit GetRaycastHit()
        {
            Physics.Raycast(_controller.transform.position + _controller.center,
                            Vector3.down,
                            out RaycastHit hitInfo,
                            _controller.height / 2 + _extraLength,
                            _layerMask);
            return hitInfo;
        }

        public bool GetRaycast()
        {
            return Physics.Raycast(_controller.transform.position + _controller.center,
                                   Vector3.down,
                                   _controller.height / 2 + _extraLength,
                                   _layerMask);

        }

        public bool GetRaycast(out RaycastHit hitInfo)
        {
            return Physics.Raycast(_controller.transform.position + _controller.center,
                                   Vector3.down,
                                   out hitInfo,
                                   _controller.height / 2 + _extraLength,
                                   _layerMask);

        }
    }
}