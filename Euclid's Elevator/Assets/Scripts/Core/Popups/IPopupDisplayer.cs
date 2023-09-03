using DeathFloor.Utilities;

namespace DeathFloor.HUD
{
    public interface IPopupDisplayer : IToggleable
    {
        public void DisplayPopup(PopupProperties popupProperties);
    }
}