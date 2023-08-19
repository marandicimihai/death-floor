using UnityEngine;
using DeathFloor.Conversion;

namespace DeathFloor.SaveSystem
{
    public class PlayerData : SaveData
    {
        public Vector3 PlayerPosition
        {
            get => Converter.FloatArrayToVector3(playerPosition);
            set => playerPosition = Converter.Vector3ToFloatArray(value);
        }

        public Quaternion PlayerRotation
        {
            get => Converter.FloatArrayToQuaternion(playerRotation);
            set => playerRotation = Converter.QuaternionToFloatArray(value);
        }

        public PlayerData(Vector3 playerPosition, Quaternion playerRotation)
        {
            PlayerPosition = playerPosition;
            PlayerRotation = playerRotation;
        }

        public PlayerData()
        {

        }
    }
}