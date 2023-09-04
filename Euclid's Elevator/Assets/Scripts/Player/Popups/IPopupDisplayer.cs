using DeathFloor.Popups;
using DeathFloor.Utilities;

namespace DeathFloor.HUD
{
    internal interface IPopupDisplayer : IToggleable
    {
        public void DisplayPopup(PopupProperties popupProperties);
    }
}