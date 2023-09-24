using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

namespace DeathFloor.Enemy
{
    internal class PatrolHandler : MonoBehaviour
    {
        [SerializeField] private List<Transform> _patrolTransforms;
        [SerializeField] private int _priorityLimit;
        [SerializeField] private int _priorityRaiseTimeInterval;
        [SerializeField] private int _priorityRaiseDelta;
        [SerializeField] private float _threshold;
        [SerializeField] private bool _debug;

        private Dictionary<Transform, int> _points;

        private Transform _currentDestination = null;

        private Func<Vector3, Vector3> _mappingFunc;
        private Transform _target;

        private void Start()
        {
            _points = new();
            foreach (Transform t in _patrolTransforms)
            {
                _points.Add(t, 0);
            }

            InvokeRepeating(nameof(RaiseAll), _priorityRaiseTimeInterval, _priorityRaiseTimeInterval);
        }

        public void SetMappingFunction(Func<Vector3, Vector3> mappingFunc)
        {
            _mappingFunc = mappingFunc;
        }

        public void SetTargetTransform(Transform transform)
        {
            _target = transform;
        }

        public void UpdatePosition(Vector3 position)
        {
            if (Vector3.Distance(_mappingFunc(position), _mappingFunc(_currentDestination.position)) < _threshold)
            {
                SetPriority(_currentDestination, 0);
                SelectNewDestination();
            }
        }

        public void Recalculate()
        {
            SelectNewDestination();
        }

        public Vector3 GetDestination()
        {
            if (_currentDestination == null) SelectNewDestination();

            return _mappingFunc(_currentDestination.position);
        }

        private void SelectNewDestination()
        {
            if (_patrolTransforms == null) return;
            IEnumerable<Transform> ienum = from pair in _points where pair.Value == GetHighestValue() select pair.Key;
            Transform selected = ChooseClosest(ienum);
            _currentDestination = selected;
        }

        private int GetHighestValue()
        {
            int highest = 0;
            foreach (int value in _points.Values)
            {
                if (highest < value)
                {
                    highest = value;
                }
            }
            return highest;
        }

        private Transform ChooseClosest(IEnumerable<Transform> transforms)
        {
            if (_target == null)
            {
                Debug.LogError("Target null"); 
                return null;
            }

            float closest = Mathf.Infinity;
            Transform closestT = null;
            foreach (Transform transform in transforms)
            {
                float curr = Vector3.Distance(_target.position, transform.position);
                if (curr < closest)
                {
                    closest = curr;
                    closestT = transform;
                }
            }

            return closestT;
        }

        public void RaisePriorityNearby(Vector3 point, float radius, int delta = 1)
        {
            foreach (Transform t in _points.Keys.ToList())
            {
                if (Vector3.Distance(t.position, point) < radius)
                {
                    SetPriority(t, _points[t] + delta);
                }
            }
        }

        private void RaiseAll()
        {
            foreach (Transform t in _points.Keys.ToList())
            {
                SetPriority(t, _points[t] + _priorityRaiseDelta);
            }
        }

        private void SetPriority(Transform key, int value)
        {
            _points[key] = Mathf.Clamp(value, 0, _priorityLimit);
        }

        private void OnDrawGizmos()
        {
            if (!_debug) return;

            foreach (Transform t in _points.Keys.ToList())
            {
                Gizmos.DrawSphere(t.position, _points[t] + 1);
            }
        }
    }
}