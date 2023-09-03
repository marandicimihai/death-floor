using UnityEngine;

namespace DeathFloor.Dialogue
{
    [CreateAssetMenu(fileName = "New Line", menuName = "Line")]
    public class LineProperties : ScriptableObject
    {
        public string Name { get => _name; }
        public string Text { get => _text; }
        public bool OneTime { get => _oneTime; }

        [SerializeField] private string _name;
        [SerializeField] private string _text;
        [SerializeField] private bool _oneTime;
    }
}