using UnityEngine;

namespace DeathFloor.HUD
{
    [CreateAssetMenu(fileName = "New Popup", menuName = "Popup")]
    public class PopupProperties : ScriptableObject
    {
        public bool OneTime { get => _oneTime; }
        public string Title { get => _title; }
        public string SubTitle { get => _subtitle; }
        public Sprite Icon { get => _icon; }
        public GameObject Prefab { get => _prefab; }

        [SerializeField] private bool _oneTime;
        [SerializeField] private string _title;
        [SerializeField] private string _subtitle;
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _prefab;
    }
}