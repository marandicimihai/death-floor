using UnityEngine;

namespace DeathFloor.Dialogue
{
    [CreateAssetMenu(fileName = "New Line", menuName = "Line")]
    public class LineProperties : ScriptableObject
    {
        public string Name;
        public string Text;
        public bool OneTime;
    }
}