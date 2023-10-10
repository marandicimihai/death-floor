using DeathFloor.Camera;
using DeathFloor.Utilities;
using System.Collections;
using UnityEngine;

namespace DeathFloor.Enemy
{
    internal class EnemyAnimationHandler : MonoBehaviour, IEnemyAnimationHandler
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _cameraRef;
        [SerializeField, RequireInterface(typeof(IVisibilityRaycastProvider))] private Object _raycastProvider;
        [SerializeField, RequireInterface(typeof(ICameraAnimationProvider))] private Object _cameraAnimationProvider;
        [SerializeField, RequireInterface(typeof(IVFXProvider))] private Object _vfxProviderObject;
        [SerializeField] private string _targetName;
        [SerializeField] private float _frontExecEADelay;
        [SerializeField] private float _backExecEADelay;
        [SerializeField] private float _frontExecBSDelay;
        [SerializeField] private float _backExecBSDelay;

        [SerializeField] private int _firstRunPose;
        [SerializeField] private int _lastRunPose;
        [SerializeField] private int _firstNormalPose;
        [SerializeField] private int _lastNormalPose;

        private IVisibilityRaycastProvider _raycast;
        private ICameraAnimationProvider _provider;
        private IVFXProvider _vfxProvider;
        private IVFX _vfx;
        private bool _switch;
        private Transform _target;

        private void Start()
        {
            _raycast = _raycastProvider as IVisibilityRaycastProvider;
            _provider = _cameraAnimationProvider as ICameraAnimationProvider;
            _vfxProvider = _vfxProviderObject as IVFXProvider;
            _vfx = _vfxProvider?.GetInterface();
            
            GameObject obj = GameObject.Find(_targetName);

            if (obj != null)
                _target = obj.transform;
        }

        private void Update()
        {
            if (_raycast?.IsVisible() ?? false)
            {
                Quaternion rotation = Quaternion.LookRotation(Vector3.Scale((_target.position - transform.position), new Vector3(1, 0, 1)).normalized, Vector3.up);
                transform.rotation = rotation * Quaternion.Euler(0, -90, 0);
            }

            if ((_raycast?.CanSeeCamera() ?? false) && _switch)
            {
                SetRunPose();
                _switch = false;
            }
            else if ((!_raycast?.CanSeeCamera() ?? true) && !_switch)
            {
                SetNormalPose();
                _switch = true;
            }
        }

        private void SetRunPose()
        {
            _animator.SetInteger("State", Random.Range(_firstRunPose, _lastRunPose + 1));
        }

        private void SetNormalPose()
        {
            _animator.SetInteger("State", Random.Range(_firstNormalPose, _lastNormalPose + 1));
        }

        public void Execute(bool visible)
        {
            if (visible)
                ExecuteFront();
            else
                ExecuteBack();
        }

        private void ExecuteFront()
        {
            _animator.SetInteger("State", -1);
            _animator.SetTrigger("Execute2");
            StartCoroutine(EnterAnimationNextFrame());
            StartCoroutine(BlackScreen(_frontExecBSDelay));
            StartCoroutine(ExitAnimation(_frontExecEADelay));
        }

        private void ExecuteBack()
        {
            _animator.SetInteger("State", -1);
            _animator.SetTrigger("Execute1");
            StartCoroutine(EnterAnimationNextFrame());
            StartCoroutine(BlackScreen(_backExecBSDelay));
            StartCoroutine(ExitAnimation(_backExecEADelay));
        }

        private IEnumerator BlackScreen(float delay)
        {
            yield return new WaitForSeconds(delay);

            _vfx.BlackScreen(true);
        }

        private IEnumerator ExitAnimation(float delay)
        {
            yield return new WaitForSeconds(delay);

            _provider?.GetInterface()?.ExitAnimation();
        }

        private IEnumerator EnterAnimationNextFrame()
        {
            yield return new WaitForEndOfFrame();

            _provider?.GetInterface()?.EnterAnimation(_cameraRef);
        }
    }
}