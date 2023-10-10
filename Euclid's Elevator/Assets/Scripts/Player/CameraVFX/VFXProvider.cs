using UnityEngine;

namespace DeathFloor.Camera
{
    internal class VFXProvider : MonoBehaviour, IVFXProvider
    {
        [SerializeField] private string _vfxObjectName;

        private IVFX _vfx;

        public IVFX GetInterface()
        {
            GameObject obj = GameObject.Find(_vfxObjectName);
            if (obj != null)
                _vfx ??= obj.GetComponent<IVFX>();

            return _vfx;
        }
    }
}