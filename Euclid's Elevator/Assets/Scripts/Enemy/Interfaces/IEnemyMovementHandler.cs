using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Enemy
{
    public interface IEnemyMovementHandler : IToggleable
    {
        public void GoTo(Vector3 position);
        public void Warp(Vector3 position);
        public void DisableForTime(float time);
        public Vector3 GetPointOnNavMesh(Vector3 point);
        public Vector3 GetPosition();
    }
}