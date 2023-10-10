using UnityEngine;

namespace DeathFloor.Enemy
{
    internal class VisibilityRaycastProvider : MonoBehaviour, IVisibilityRaycastProvider
    {
        [SerializeField] private Transform[] _visibilityPoints;
        [SerializeField] private Collider _collider;
        private UnityEngine.Camera _mainCamera;

        private void Start()
        {
            //can cause camera problems later but it works for now...
            _mainCamera = UnityEngine.Camera.main;
        }

        public bool IsVisible()
        {
            Vector3 camPos = UnityEngine.Camera.main.transform.position;

            bool result = false;
            for (int i = 0; i < _visibilityPoints.Length; i++)
            {
                if (Physics.Raycast(camPos, _visibilityPoints[i].position - camPos, out RaycastHit info) &&
                    Vector3.Distance(camPos, info.point) >= Vector3.Distance(camPos, _visibilityPoints[i].position) &&
                    GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(_mainCamera), _collider.bounds))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool CanSeeCamera()
        {
            Vector3 camPos = UnityEngine.Camera.main.transform.position;

            bool result = false;
            for (int i = 0; i < _visibilityPoints.Length; i++)
            {
                if (Physics.Raycast(camPos, _visibilityPoints[i].position - camPos, out RaycastHit info) &&
                    Vector3.Distance(camPos, info.point) >= Vector3.Distance(camPos, _visibilityPoints[i].position))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool GetRaycast()
        {
            throw new System.NotImplementedException();
        }

        public bool GetRaycast(out RaycastHit hitInfo)
        {
            throw new System.NotImplementedException();
        }

        public RaycastHit GetRaycastHit()
        {
            throw new System.NotImplementedException();
        }
    }
}