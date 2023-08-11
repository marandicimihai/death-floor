using UnityEngine;
using DeathFloor.Conversion;

namespace DeathFloor.SaveSystem
{
    public class EnemyData : SaveData
    {
        public Vector3 EnemyPosition
        {
            get => Converter.FloatArrayToVector3(enemyPosition);
            set => enemyPosition = Converter.Vector3ToFloatArray(value);
        }

        public bool Spawned
        {
            get => enemySpawned;
            set => enemySpawned = value;
        }

        public EnemyData(Vector3 position, bool spawned)
        {
            EnemyPosition = position;
            Spawned = spawned;
        }
    }
}