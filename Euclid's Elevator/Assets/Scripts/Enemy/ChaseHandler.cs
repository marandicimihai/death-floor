using System;
using UnityEngine;

namespace DeathFloor.Enemy
{
    internal class ChaseHandler : MonoBehaviour
    {
        [SerializeField] private string _targetName;

        private Transform _target;

        private Func<Vector3, Vector3> _mappingFunc;

        public void SetMappingFunction(Func<Vector3, Vector3> mappingFunc)
        {
            _mappingFunc = mappingFunc;
        }

        private void Start()
        {
            _target = GameObject.Find(_targetName).transform;
        }

        public Vector3 GetDestination()
        {
            return _mappingFunc(_target.position);
        }
    }
}