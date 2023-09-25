using DeathFloor.Movement;
using DeathFloor.Utilities;
using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class SodaHelper : MonoBehaviour, IUseHelper
    {
        public UseTag TargetUseTag => _targetTag;

        [SerializeField, RequireInterface(typeof(IMovementApplier))] private Object _movementApplier;
        [SerializeField] private UseTag _targetTag;

        private IMovementApplier _applier;

        private void Start()
        {
            _applier = _movementApplier as IMovementApplier;            
        }

        public void UseExtension(GameObject rootObject)
        {
            if (!rootObject.TryGetComponent(out Soda soda)) return;
            _applier.BoostForTime(soda.Time, soda.Multiplier);
        }
    }
}