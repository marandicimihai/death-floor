using UnityEngine;

namespace DeathFloor.Journal
{
    [CreateAssetMenu(fileName = "New Page", menuName = "Page")]
    public class PageProperties : ScriptableObject
    {
        public string Name { get => _name; }
        public GameObject Prefab { get => _prefab; }

        [SerializeField] private string _name;
        [SerializeField] private GameObject _prefab;
    }
}