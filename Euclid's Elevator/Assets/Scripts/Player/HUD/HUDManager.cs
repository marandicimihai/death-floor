using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.HUD
{
    internal class HUDManager : MonoBehaviour, IHUDManager
    {
        [SerializeField, RequireInterface(typeof(IToggleable))] private Object _dialogue;
        [SerializeField, RequireInterface(typeof(IToggleable))] private Object _actionInfo;
        [SerializeField, RequireInterface(typeof(IToggleable))] private Object _popup;
        [SerializeField, RequireInterface(typeof(IToggleable))] private Object _crosshair;

        IToggleable[] _toggleables;

        private void Start()
        {
            _toggleables = new IToggleable[]
            {
                _dialogue as IToggleable,
                _actionInfo as IToggleable,
                _popup as IToggleable,
                _crosshair as IToggleable
            };
        }

        public void EnableHUD()
        {
            foreach (var toggleable in _toggleables)
            {
                toggleable.Enable();
            }
        }

        public void DisableHUD()
        {
            foreach (var toggleable in _toggleables)
            {
                toggleable.Disable();
            }
        }
    }
}