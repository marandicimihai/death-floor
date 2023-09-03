using DeathFloor.Utilities;

namespace DeathFloor.HUD
{
    public interface IPopupManager : IToggleable
    {
        public void ShowPopup(PopupProperties popupProperties);
    }
}