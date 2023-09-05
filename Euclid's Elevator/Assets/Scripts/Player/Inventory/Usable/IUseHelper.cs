using UnityEngine;

namespace DeathFloor.Inventory
{
    public interface IUseHelper
    {
        public UseTag TargetUseTag { get; }

        public void UseExtension(GameObject rootObject);
    }
}