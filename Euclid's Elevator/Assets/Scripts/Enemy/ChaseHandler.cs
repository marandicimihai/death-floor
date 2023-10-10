using DeathFloor.Player;
using System;
using UnityEngine;

namespace DeathFloor.Enemy
{
    internal class ChaseHandler : MonoBehaviour
    {
        public float KillDistance { get => _killDistance; }

        [SerializeField] private float _killDistance;
        [SerializeField] private string _targetName;

        private Transform _target;

        private Func<Vector3, Vector3> _mappingFunc;

        private IPlayer _player;

        public void SetMappingFunction(Func<Vector3, Vector3> mappingFunc)
        {
            _mappingFunc = mappingFunc;
        }

        private void Start()
        {
            _target = GameObject.Find(_targetName).transform;
        }

        public IPlayer GetPlayerInterface()
        {
            _player ??= _target.GetComponent<IPlayer>();
            return _player;
        }

        public Vector3 GetDestination()
        {
            return _mappingFunc(_target.position);
        }
    }
}