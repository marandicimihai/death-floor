using System.Reflection;

namespace DeathFloor.SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        #region PlayerData
        public float[] playerPosition;
        public float[] playerRotation;
        #endregion
        #region CameraData
        public float[] cameraRotation;
        #endregion
        #region InventoryData
        public string[] holdingitems;
        public int[] holdingLengths;
        public string[] holdingVariables;
        #endregion
        #region GameManagerData
        public int stage;
        public int gameStage;
        #endregion
        #region ElevatorData
        public bool broken;
        public bool waiting;
        #endregion
        #region EnemyData
        public float[] enemyPosition;
        public bool enemySpawned;
        #endregion
        #region ItemData
        public string[] spawneditems;
        public float[] spawneditemPositions;
        public int[] spawnedlengths;
        public string[] spawnedvariables;
        #endregion
        #region TrapData
        public float[] trapPositions;
        #endregion
        #region PopUpData
        public string[] usedPopUps;
        #endregion
        #region DialogueData
        public string[] usedLines;
        #endregion
        #region JournalData
        public string[] pages;
        #endregion
        #region InsanityData
        public float insanity;
        #endregion

        public SaveData()
        {

        }

        public void CopyData(SaveData data)
        {
            foreach (FieldInfo field in typeof(SaveData).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                field.SetValue(this, field.GetValue(data));
            }
        }

        public static SaveData operator +(SaveData left, SaveData right)
        {
            SaveData data = new();

            foreach (FieldInfo field in typeof(SaveData).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                //Use the new class for default values
                if (field.GetValue(left) == field.GetValue(data) &&
                    field.GetValue(right) != field.GetValue(data))
                {
                    field.SetValue(left, field.GetValue(right));
                }
            }

            return left;
        }
    }
}