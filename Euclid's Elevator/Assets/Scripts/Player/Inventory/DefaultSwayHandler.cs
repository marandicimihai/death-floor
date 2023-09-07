using DeathFloor.Input;
using UnityEngine;

namespace DeathFloor.Inventory
{
    internal class DefaultSwayHandler : MonoBehaviour, ISwayHandler
    {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private float _swayStep;
        [SerializeField] private float _swayMaxStep;
        [SerializeField] private float _swaySmooth;
        [SerializeField] private float _swayRotStep;
        [SerializeField] private float _swayMaxRotStep;
        [SerializeField] private float _swaySmoothRot;

        public void PerformSway(GameObject itemRootObject, ItemProperties properties)
        {
            Vector2 invertLook = _inputReader.Look * -_swayStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -_swayMaxStep, _swayMaxStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -_swayMaxStep, _swayMaxStep);

            Vector3 swayPos = invertLook;

            Vector3 swayRotLook = _inputReader.Look * -_swayRotStep;
            swayRotLook.x = Mathf.Clamp(swayRotLook.x, -_swayMaxRotStep, _swayMaxRotStep);
            swayRotLook.y = Mathf.Clamp(swayRotLook.y, -_swayMaxRotStep, _swayMaxRotStep);

            Vector3 swayRot = new(swayRotLook.y, swayRotLook.x, swayRotLook.x);

            itemRootObject.transform.SetLocalPositionAndRotation(
                Vector3.Lerp(
                    itemRootObject.transform.localPosition, 
                    swayPos + properties.OffsetPosition, 
                    Time.deltaTime * _swaySmooth), 
                Quaternion.Slerp(itemRootObject.transform.localRotation, 
                    Quaternion.Euler(swayRot + properties.OffsetRotation), 
                    Time.deltaTime * _swaySmoothRot));
        }
    }
}