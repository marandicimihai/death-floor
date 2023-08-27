using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Interactions
{
    internal class InteractionRaycastProvider : MonoBehaviour, IRaycastProvider
    {
        [Tooltip("Preferably the camera")]
        [SerializeField] private Transform _viewPoint;

        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _layerMask;

        public bool GetRaycast()
        {
            return Physics.Raycast(_viewPoint.position,
                                   _viewPoint.forward,
                                   _rayDistance,
                                   _layerMask);
        }

        public bool GetRaycast(out RaycastHit hitInfo)
        {
            return Physics.Raycast(_viewPoint.position,
                                   _viewPoint.forward,
                                   out hitInfo,
                                   _rayDistance,
                                   _layerMask);
        }

        public RaycastHit GetRaycastHit()
        {
            Physics.Raycast(_viewPoint.position,
                            _viewPoint.forward,
                            out RaycastHit hitInfo,
                            _rayDistance,
                            _layerMask);
            return hitInfo;
        }
    }
}