using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class DefaultDropHandler : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Transform _dropPointAndDirection;
        [SerializeField] private float _dropForce;

        public void DropItem(IItem itemToDrop)
        {
            GameObject dropped = Instantiate(itemToDrop.Properties.PhysicalModel, _dropPointAndDirection.position, Quaternion.identity);

            if (dropped.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(_dropPointAndDirection.forward * _dropForce, ForceMode.Impulse);
            }
            if (dropped.TryGetComponent(out IItem item))
            {
                item.SetValuesRuntime(itemToDrop);
            }

            Destroy(itemToDrop.GetRoot());
        }
    }
}