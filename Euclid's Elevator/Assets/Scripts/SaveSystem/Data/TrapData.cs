using UnityEngine;
using DeathFloor.Conversion;

namespace DeathFloor.SaveSystem
{
    public class TrapData : SaveData
    {
        public Vector3[] TrapPositions
        {
            get => Converter.FloatArrayToVector3Array(trapPositions);
            set => trapPositions = Converter.Vector3ArrayToFloatArray(value);
        }

        public TrapData(Vector3[] positions)
        {
            TrapPositions = positions;
        }
    }
}