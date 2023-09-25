using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace DeathFloor.Enemy
{
    internal class DefaultEnemyMovementHandler : MonoBehaviour, IEnemyMovementHandler
    {
        [SerializeField] private NavMeshAgent _agent;
        private bool _canMove;

        private float _timeToReenable;

        public void Disable()
        {
            _canMove = false;
            _agent.enabled = false;
        }

        public void Enable()
        {
            _canMove = true;
            _agent.enabled = true;
        }

        public void GoTo(Vector3 position)
        {
            if (!_canMove || position == _agent.destination) return;

            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 100f, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }

        public void Warp(Vector3 position)
        {
            _agent.Warp(position);
        }

        public Vector3 GetPointOnNavMesh(Vector3 point)
        {
            if (NavMesh.SamplePosition(point, out NavMeshHit hit, 100f, NavMesh.AllAreas))
            {
                return hit.position;
            }
            else throw new Exception("Point too far.");
        }

        public Vector3 GetPosition()
        {
            return GetPointOnNavMesh(_agent.transform.position);
        }

        public void DisableForTime(float time)
        {
            if (Time.time + time < _timeToReenable)
            {
                return;
            }
            _timeToReenable = Time.time + time;
            CancelInvoke(nameof(Enable));
            Invoke(nameof(Enable), time);
        }

        public float GetPathLength(Vector3 destination)
        {
            NavMeshPath path = new();
            _agent.CalculatePath(destination, path);
            float length = 0;
            Vector3[] corners = path.corners;
            if (corners.Length >= 2)
            {
                for (int i = 1; i < corners.Length; i++)
                {
                    length += Vector3.Distance(corners[i], corners[i - 1]);
                }
            }
            return length;
        }
    }
}