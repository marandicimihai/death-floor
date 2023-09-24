using System;
using UnityEngine;

namespace DeathFloor.Enemy
{
    internal class InspectHandler : MonoBehaviour
    {
        public bool Inspecting { get; private set; }
        public float InspectAlertRadius { get => _inspectAlertRadius; }

        [SerializeField] private float _inspectAlertRadius;
        [SerializeField] private float _threshold;

        private bool _playerInspect;
        private Vector3 _destination;

        private Func<Vector3, Vector3> _mappingFunc;

        public void SetMappingFunction(Func<Vector3, Vector3> mappingFunc)
        {
            _mappingFunc = mappingFunc;
        }

        public void UpdatePosition(Vector3 agentPosition)
        {
            if (Vector3.Distance(_mappingFunc(_destination), _mappingFunc(agentPosition)) < _threshold)
            {
                Inspecting = false;
            }
        }

        public Vector3 GetDestination()
        {
            if (!Inspecting) throw new Exception("Not inspecting.");

            return _mappingFunc(_destination);
        }

        public void Inspect(Vector3 position, bool playerInspect = false)
        {
            if (_playerInspect && !playerInspect) return;

            Inspecting = true;

            _playerInspect = playerInspect;
            _destination = position;
        }
    }
}