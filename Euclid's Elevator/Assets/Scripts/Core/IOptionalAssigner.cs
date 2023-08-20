using UnityEngine;

namespace DeathFloor.Utilities
{
    public interface IOptionalAssigner
    {
        public T AssignUsingGetComponent<T>(Optional<MonoBehaviour> assignFrom) where T : class;
    }
}