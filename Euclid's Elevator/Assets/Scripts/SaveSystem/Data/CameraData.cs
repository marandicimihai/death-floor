using UnityEngine;
using DeathFloor.Conversion;

namespace DeathFloor.SaveSystem
{
    public class CameraData : SaveData
    {
        public Vector2 CameraRotation
        {
            get => Converter.FloatArrayToVector2(cameraRotation);
            set => cameraRotation = Converter.Vector2ToFloatArray(value);
        }

        public CameraData(Vector2 cameraRotation)
        {
            CameraRotation = cameraRotation;
        }

        public CameraData()
        {

        }
    }
}