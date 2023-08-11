namespace DeathFloor.SaveSystem
{

    [System.Serializable]
    public class SaveData
    {
        #region PlayerData
        protected float[] playerPosition;
        protected float[] playerRotation;
        #endregion
        #region CameraData
        protected float[] cameraRotation;
        #endregion
        #region InventoryData
        protected string[] holdingitems;
        protected int[] holdingLengths;
        protected string[] holdingVariables;
        #endregion
        #region GameManagerData
        protected int stage;
        protected int gameStage;
        #endregion
        #region ElevatorData
        protected bool broken;
        protected bool waiting;
        #endregion
        #region EnemyData
        protected float[] enemyPosition;
        protected bool enemySpawned;
        #endregion
        #region ItemData
        protected string[] spawneditems;
        protected float[] spawneditemPositions;
        protected int[] spawnedlengths;
        protected string[] spawnedvariables;
        #endregion
        #region TrapData
        protected float[] trapPositions;
        #endregion
        #region PopUpData
        protected string[] usedPopUps;
        #endregion
        #region DialogueData
        protected string[] usedLines;
        #endregion
        #region JournaldData
        protected string[] pages;
        #endregion
        #region InsanityData
        protected float insanity;
        #endregion
    }
}