using System.Collections.Generic;
using UnityEngine;
using System;

enum GameStage
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

    public Transform playerTransform;
    public Player player;
    public EnemyNavigation enemy;
    [SerializeField] int stageCount;
    [SerializeField] Transform playerSpawn;
    [SerializeField] EnemySpawn[] enemySpawns;

    public EventHandler OnStageStart;
    public EventHandler OnDeath;

    GameStage stage;

    private void Awake()
    {
        Instance = this;
        StartGame();
    }

    private void Update()
    {
        if (stage == GameStage.Loading)
        {

        }
        else if (stage == GameStage.Lobby)
        {

        }
        else if (stage == GameStage.SecondLoading)
        {

        }
        else if (stage == GameStage.GameLevel)
        {

        }
        else if (stage == GameStage.WaitForPlayer)
        {
            
        }
        else if (stage == GameStage.End)
        {

        }
    }

    #region GameLevel

    void StartGame()
    {
        stage = GameStage.GameLevel;
        Stage = 1;

        OnStageStart?.Invoke(this, new EventArgs());

        SpawnPlayer();
        SpawnEnemy();
    }

    public void PlayerDeath(int deaths, int maxDeaths)
    {
        if (deaths == maxDeaths)
        {
            stage = GameStage.End;
        }
        else
        {
            OnDeath?.Invoke(this, new EventArgs());
            SpawnPlayer();
            SpawnEnemy();
        }
    }

    public void PlayerEnteredElevator()
    {
        if (stage == GameStage.WaitForPlayer)
        {
            NextStage();
        }
    }

    public void KeycardInserted()
    {
        if (stage == GameStage.GameLevel)
        {
            stage = GameStage.WaitForPlayer;
        }
    }

    void NextStage()
    {
        Stage++;
        if (Stage > stageCount)
        {
            stage = GameStage.End;
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