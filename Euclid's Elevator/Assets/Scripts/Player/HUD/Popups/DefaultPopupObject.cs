using UnityEngine;
using UnityEngine.UI;

namespace DeathFloor.HUD
{
    internal class DefaultPopupObject : MonoBehaviour, IPopupObject
    {
        [SerializeField] private Text _title;
        [SerializeField] private Text _subtitle;
        [SerializeField] private Image _icon;

        public void Initialize(string title, string subtitle, Sprite icon)
        {
            _title.text = title;
            _subtitle.text = subtitle;
            _icon.sprite = icon;
        }
    }
}