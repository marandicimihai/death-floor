using UnityEngine;

namespace DeathFloor.Camera
{
    internal class CameraAnimationProvider : MonoBehaviour, ICameraAnimationProvider
    {
        [SerializeField] private string _objectName;

        private ICameraAnimation _provider;

        public ICameraAnimation GetInterface()
        {
            if (_provider == null)
            {
                var obj = GameObject.Find(_objectName);
                
                if (obj != null)
                    _provider = obj.GetComponent<ICameraAnimation>();
            }
            
            return _provider;
        }
    }
}