using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class Soda : CollectableItem, IUsable
    {
        public UseTag UseTag { get => _useTag; }
        public float Multiplier { get => _multiplier; }
        public float Time { get => _time; }

        [SerializeField] private UseTag _useTag;
        [SerializeField] private float _multiplier;
        [SerializeField] private float _time;

        public void OnUse()
        {
        }
    }
}