using UnityEngine;

namespace DeathFloor.Utilities
{
    public class OptionalAssigner : IOptionalAssigner
    {
        private readonly MonoBehaviour _classInstance;

        public OptionalAssigner(MonoBehaviour classInstance)
        {
            _classInstance = classInstance;
        }

        public T AssignUsingGetComponent<T>(Optional<MonoBehaviour> assignFrom) where T : class
        {
            if (assignFrom.Enabled)
            {
                return assignFrom.Value as T;
            }
            try
            {
                return _classInstance.GetComponent<T>();
            }
            catch { return default; }
        }

        public T AssignUsingGetComponent<T>(Optional<T> assignFrom) where T : class
        {
            if (assignFrom.Enabled)
            {
                return assignFrom.Value as T;
            }
            try
            {
                return _classInstance.GetComponent<T>();
            }
            catch { return default; }
        }
    }
}