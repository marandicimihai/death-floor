using UnityEngine;

namespace DeathFloor.HUD
{
    public interface IPopupObject
    {
        public void Initialize(string title, string subtitle, Sprite icon);
    }
}