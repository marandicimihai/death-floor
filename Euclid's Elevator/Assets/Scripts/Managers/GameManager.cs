using System.Collections.Generic;
using UnityEngine;
using System;
using DeathFloor.SaveSystem;

public enum GameStage
{
    Tutorial,
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

public class GameManager : MonoBehaviour, ISaveData<GameManagerData>
{
    public static GameManager Instance { get; private set; }
    public int Stage { get; private set; }
    public GameStage GameStage { get; private set; }

    public bool CanSave => true;

    public Transform playerTransform;
    public Player player;
    public EnemyNavigation enemy;
    [SerializeField] int stageCount;
    [SerializeField] Transform playerSpawn;
    [SerializeField] EnemySpawn[] enemySpawns;

    public EventHandler OnElevatorDoorClosed;
    public EventHandler OnDeath;
    public EventHandler OnGameOver;
    public EventHandler OnGameWin;


    void Awake()
    {
        Instance = this;
        /*Inventory.OnPickUpKeycard += (object caller, EventArgs args) =>
        {
            if (GameStage == GameStage.Tutorial && player.Deaths == 0)
            {
                SpawnEnemy();
            }
        };*/
        player.PlayerDied += PlayerDeath;
        Stage = 1;
    }

    public void OnFirstTimeLoaded()
    {
        StartTutorial();
    }

    public GameManagerData OnSaveData()
    {
        return new GameManagerData(Stage, (int)GameStage);
    }

    public void LoadData(GameManagerData data)
    {
        Stage = data.Stage;
        GameStage = (GameStage)data.GameStage;
    }

    void HideEnemy()
    {
        enemy.gameObject.SetActive(false);
    }

    #region GameLevel

    void StartTutorial()
    {
        GameStage = GameStage.Tutorial;
        Stage = 1;

        OnElevatorDoorClosed?.Invoke(this, new EventArgs());

        HideEnemy();
        SpawnPlayer();
    }

    public void PlayerDeath(int deaths, int maxDeaths)
    {
        if (deaths == maxDeaths)
        {
            OnGameOver?.Invoke(this, new EventArgs());
            GameStage = GameStage.End;
        }
        else
        {
            OnDeath?.Invoke(this, new EventArgs());
            SpawnPlayer();

            if (GameStage == GameStage.GameLevel)
            {
                SpawnEnemy();
            }
            else if (GameStage == GameStage.Tutorial)
            {
                HideEnemy();
            }
        }
    }

    public void ElevatorRideInitialized()
    {
        if (GameStage == GameStage.WaitForPlayer)
        {
            GameStage = GameStage.GameLevel;
            NextStage();
        }
    }

    public void KeycardInserted()
    {
        GameStage = GameStage.WaitForPlayer;
    }

    void NextStage()
    {
        Stage++;
        if (Stage > stageCount)
        {
            OnGameWin?.Invoke(this, new EventArgs());
            GameStage = GameStage.End;
        }
        else
        {
            OnElevatorDoorClosed?.Invoke(this, new EventArgs());
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

        enemy.gameObject.SetActive(true);
        enemy.Spawn(spawns[UnityEngine.Random.Range(0, spawns.Count)].spawn.position);
    }

    #endregion
}