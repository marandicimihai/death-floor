using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameStage
{
    Loading,
    Lobby,
    SecondLoading,
    GameLevel,
    WaitForPlayer,
    End
}

[System.Serializable]
struct EnemySpawn
{
    public Transform spawn;
    public int stage;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int Stage { get; private set; }
    public GameStage GameStage { get; private set; }

    public Transform playerTransform;
    public Player player;
    public EnemyNavigation enemy;
    [SerializeField] int stageCount;
    [SerializeField] Transform playerSpawn;
    [SerializeField] EnemySpawn[] enemySpawns;

    public EventHandler OnStageStart;
    public EventHandler OnDeath;


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (GameStage == GameStage.Loading)
        {

        }
        else if (GameStage == GameStage.Lobby)
        {

        }
        else if (GameStage == GameStage.SecondLoading)
        {

        }
        else if (GameStage == GameStage.GameLevel)
        {

        }
        else if (GameStage == GameStage.WaitForPlayer)
        {
            
        }
        else if (GameStage == GameStage.End)
        {

        }
    }

    #region GameLevel

    void StartGame()
    {
        GameStage = GameStage.GameLevel;
        Stage = 1;

        OnStageStart?.Invoke(this, new EventArgs());

        SpawnPlayer();
        SpawnEnemy();
    }

    public void PlayerDeath(int deaths, int maxDeaths)
    {
        if (deaths == maxDeaths)
        {
            GameStage = GameStage.End;
        }
        else
        {
            OnDeath?.Invoke(this, new EventArgs());
            SpawnPlayer();
            SpawnEnemy();
        }
    }

    public void ElevatorRideInitialized()
    {
        if (GameStage == GameStage.WaitForPlayer)
        {
            NextStage();
        }
    }

    public void KeycardInserted()
    {
        if (GameStage == GameStage.GameLevel)
        {
            GameStage = GameStage.WaitForPlayer;
        }
    }

    void NextStage()
    {
        Stage++;
        if (Stage > stageCount)
        {
            GameStage = GameStage.End;
        }
        else
        {
            OnStageStart?.Invoke(this, new EventArgs());
            SpawnEnemy();
        }
    }

    void SpawnPlayer()
    {
        player.Spawn(playerSpawn.position);
    }

    void SpawnEnemy()
    {
        List<EnemySpawn> spawns = new();

        if (Stage == stageCount)
        {
            spawns.AddRange(enemySpawns);
        }
        else
        {
            foreach (EnemySpawn spawn in enemySpawns)
            {
                if (spawn.stage == Stage)
                {
                    spawns.Add(spawn);
                }
            }
        }

        enemy.Spawn(spawns[UnityEngine.Random.Range(0, spawns.Count)].spawn.position);
    }

    #endregion
}