using UnityEngine;

namespace DeathFloor.HUD
{
    internal interface IPopupObject
    {
        public void Initialize(string title, string subtitle, Sprite icon);
    }
}