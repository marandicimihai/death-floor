[System.Serializable]
public class GameData
{
    public float[] PlayerPosition;
    public float[] PlayerRotation;
    public float[] CameraRotation;
    public string[] holdingitems;
    public int[] lengths;
    public string[] variables;
    public int stage = -1;
    public int gameStage = -1;
    public bool broken;
    public bool waiting;
    public bool canClose;
    public float[] EnemyPosition;
    public bool enemySpawned;

    public string[] spawneditems;
    public float[] spawneditemPositions;
    public int[] spawnedlengths;
    public string[] spawnedvariables;

    public bool[] locked;
    public bool[] open;

    public int trapCount;
    public float[] trapPositions;

    public string[] usedPopUps;
    public string[] usedLines;
    public string[] pages;

    public float insanity;
}