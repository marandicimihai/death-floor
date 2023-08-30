using UnityEngine;

namespace DeathFloor.Utilities
{
    public interface IOptionalAssigner
    {
        /// <summary>
        /// Uses the optional variable, otherwise uses GetComponent on the current object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assignFrom"></param>
        /// <returns></returns>
        public T AssignUsingGetComponent<T>(Optional<MonoBehaviour> assignFrom) where T : class;

        /// <summary>
        /// Uses the optional variable, otherwise uses GetComponent on the current object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assignFrom"></param>
        /// <returns></returns>
        public T AssignUsingGetComponent<T>(Optional<T> assignFrom) where T : class;
    }
}