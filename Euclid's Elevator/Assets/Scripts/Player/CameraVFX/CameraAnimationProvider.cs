using UnityEngine;

namespace DeathFloor.Camera
{
    internal class CameraAnimationProvider : MonoBehaviour, ICameraAnimationProvider
    {
        [SerializeField] private string _objectName;

        private ICameraAnimation _provider;

        private void Start()
        {
            _provider ??= GameObject.Find(_objectName).GetComponent<ICameraAnimation>();
        }

        public ICameraAnimation GetInterface()
        {
            _provider ??= GameObject.Find(_objectName).GetComponent<ICameraAnimation>();
            
            return _provider;
        }
    }
}