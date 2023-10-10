using DeathFloor.Animation.Triggers;
using DeathFloor.Utilities;
using System.Collections;
using UnityEngine;

namespace DeathFloor.Enemy
{
    internal class DefaultEnemy : MonoBehaviour, IEnemy
    {
        public bool IsVisible { get => _visibility?.GetRaycast() ?? false; }

        [SerializeField, RequireInterface(typeof(IEnemyMovementHandler))] private Object _movementHandler;
        [SerializeField, RequireInterface(typeof(IRaycastProvider))] private Object _visibilityRaycast;
        [SerializeField, RequireInterface(typeof(IEnemyAnimationHandler))] private Object _animationHandler;
        [SerializeField, RequireInterface(typeof(IPlayerSpawnTrigger))] private Object _spawnTriggerObject;
        [SerializeField] private PatrolHandler _patrolHandler;
        [SerializeField] private InspectHandler _inspectHandler;
        [SerializeField] private ChaseHandler _chaseHandler;
        [SerializeField] private float _spawnDelay;

        private IEnemyMovementHandler _movement;
        private IVisibilityRaycastProvider _visibility;
        private IEnemyAnimationHandler _animation;
        private IPlayerSpawnTrigger _spawnTrigger;

        private bool _wasChasing;

        private void Start()
        {
            _movement = _movementHandler as IEnemyMovementHandler;
            _visibility = _visibilityRaycast as IVisibilityRaycastProvider;
            _animation = _animationHandler as IEnemyAnimationHandler;
            _spawnTrigger = _spawnTriggerObject as IPlayerSpawnTrigger;

            _patrolHandler.SetMappingFunction(_movement.GetPointOnNavMesh);
            _patrolHandler.SetLengthFunction(_movement.GetPathLength);
            _inspectHandler.SetMappingFunction(_movement.GetPointOnNavMesh);
            _chaseHandler.SetMappingFunction(_movement.GetPointOnNavMesh);

            Enable();
        }

        private void Update()
        {
            if (_visibility?.CanSeeCamera() ?? false)
            {
                Vector3 target = _chaseHandler.GetDestination();
                if (!_visibility?.IsVisible() ?? false)
                {
                    _movement.Enable();
                    _movement.GoTo(target);
                }
                else
                {
                    _movement.Disable();
                }

                _wasChasing = true;

                if (Vector3.Distance(target, _movement.GetPosition()) < _chaseHandler.KillDistance)
                {
                    _wasChasing = false;

                    if ((!_chaseHandler.GetPlayerInterface()?.Dead ?? false))
                    {
                        _chaseHandler.GetPlayerInterface()?.Die();
                        _animation?.Execute(_visibility?.IsVisible() ?? true);
                        StartCoroutine(InvokeSpawn());
                    }
                }
            }
            else if (_wasChasing)
            {
                Vector3 target = _chaseHandler.GetDestination();
                Inspect(target, true);
                _wasChasing = false;
            }
            else if (_inspectHandler.Inspecting)
            {
                _movement.Enable();
                _movement.GoTo(_inspectHandler.GetDestination());
                _inspectHandler.UpdatePosition(_movement.GetPosition());
            }
            else
            {
                _movement.Enable();
                _movement.GoTo(_patrolHandler.GetDestination());
                _patrolHandler.UpdatePosition(_movement.GetPosition());
            }
        }

        public void Inspect(Vector3 position, bool playerInspect)
        {
            if (!playerInspect)
            {
                _patrolHandler.RaisePriorityNearby(transform.position, _inspectHandler.InspectAlertRadius);
            }
            else
            {
                _patrolHandler.RaisePriorityNearby(transform.position + (position - transform.position).normalized * (_inspectHandler.InspectAlertRadius / 2), _inspectHandler.InspectAlertRadius);
            }

            _inspectHandler.Inspect(position, playerInspect);
        }

        private IEnumerator InvokeSpawn()
        {
            yield return new WaitForSeconds(_spawnDelay);

            _spawnTrigger?.Spawn();
        }

        public void Disable()
        {
            _movement?.Disable();
        }

        public void Enable()
        {
            _movement?.Enable();
        }
    }
}
