using DeathFloor.Utilities;

namespace DeathFloor.Popups
{
    public interface IPopupManager : IToggleable
    {
        public void ShowPopup(PopupProperties popupProperties);
    }
}