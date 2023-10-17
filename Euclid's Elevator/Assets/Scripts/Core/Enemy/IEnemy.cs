using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Enemy
{
    public interface IEnemy : IToggleable
    {
        public bool IsVisible { get; }
        public void Inspect(Vector3 position, bool playerInspect);
    }
}