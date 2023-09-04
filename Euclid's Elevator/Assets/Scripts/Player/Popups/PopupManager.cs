using DeathFloor.Utilities;
using System.Collections.Generic;
using DeathFloor.HUD;
using UnityEngine;

namespace DeathFloor.Popups
{
    internal class PopupManager : MonoBehaviour, IPopupManager
    {
        [SerializeField, RequireInterface(typeof(IPopupDisplayer))] private Object _popupDisplayer;

        private List<PopupProperties> _used;
        private IPopupDisplayer _displayer;
        private Utilities.Logger.ILogger _logger;
        private bool _canDisplay;

        private void Start()
        {
            _displayer = _popupDisplayer as IPopupDisplayer;

            _used ??= new List<PopupProperties>();

            Enable();

            Debug.LogError("Fix logger here.");
        }

        public void ShowPopup(PopupProperties popupProperties)
        {
            if (!_canDisplay) return;

            if (popupProperties.OneTime && _used.Contains(popupProperties))
            {
                _logger.Debug($"The popup {_logger.Bold(popupProperties.Title)} has already been {_logger.Italic("displayed")}.");
                return;
            }

            _displayer?.DisplayPopup(popupProperties);

            if (popupProperties.OneTime)
            {
                _used.Add(popupProperties);
            }
        }

        public void Disable()
        {
            _canDisplay = false;
            _displayer.Disable();
        }

        public void Enable()
        {
            _displayer.Enable();
            _canDisplay = true;
        }
    }
}