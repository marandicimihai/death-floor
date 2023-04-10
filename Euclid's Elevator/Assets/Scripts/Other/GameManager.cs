using System.Collections;
using UnityEngine;
using UnityEditor;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("References")]
    public Transform player;
    public FpsController playerController;
    public Transform enemy;
    public Enemy enemyController;
    public Elevator elevator;
    public bool ElevatorOpen 
    {
        get
        {
            return elevator.Open;
        }
    }

    [Header("Death Settings")]
    [SerializeField] float timeAfterDeath;
    [SerializeField] float timeUntilOpenElevator;
    [SerializeField] float unlockPlayerTime;

    [Header("Elevator")]
    [SerializeField] Vector3 elevatorPoint;
    [SerializeField] float elevatorRadius;

    [Header("Spawn Settings")]
    [SerializeField] int maxDeaths;
    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform[] oogaManSpawns;
    [SerializeField] float shakeTime;
    [SerializeField] float shakeMag;

    [Header("Game Settings")]
    [SerializeField] int stages;

    public event EventHandler<StageArgs> OnStageStart;
    
    public EventHandler OnSpawn;
    public EventHandler<DeathArgs> OnDeath;
    public EventHandler OnEnd;

    bool waitingForPlayer;

    int stage;
    int deaths;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    //called once
    void StartGame()
    {
        SpawnPlayer();
        SpawnEnemy();
        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            SoundManager.instance.PlaySound("ElevatorStop");
            SoundManager.instance.StopSound("ElevatorHum");
            elevator.OpenElevator();
        }));

        stage = 1;
        OnStageStart?.Invoke(this, new StageArgs(stage));

        OnSpawn?.Invoke(this, new EventArgs());
    }
    //respawns player and enemy
    public void Die()
    {
        elevator.CloseElevator();
        elevator.BreakDown();
        enemyController.Stop();

        StartCoroutine(WaitAndExec(timeAfterDeath, () =>
        {
            deaths++;
            if (deaths >= maxDeaths)
            {
                OnEnd?.Invoke(this, new EventArgs());
                //END MENU
            }
            else
            {
                OnDeath?.Invoke(this, new DeathArgs(deaths));
                SpawnPlayer();
                SpawnEnemy();
                StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
                {
                    SoundManager.instance.PlaySound("ElevatorStop");
                    SoundManager.instance.StopSound("ElevatorHum");
                    elevator.OpenElevator();
                }));
            }
        }));
    }
    //moves on to next stage
    IEnumerator NextStage()
    {
        waitingForPlayer = true;
        while (Vector3.Distance(elevatorPoint, player.position) > elevatorRadius)
        {
            yield return null;
        }

        elevator.CloseElevator();
        SoundManager.instance.PlaySound("ElevatorHum");
        SoundManager.instance.StopSound("Hum");
        StartCoroutine(playerController.cameraController.Shake(shakeTime, shakeMag));

        stage++;
        OnStageStart?.Invoke(this, new StageArgs(stage));

        SpawnEnemy();
        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            SoundManager.instance.PlaySound("ElevatorStop");
            SoundManager.instance.StopSound("ElevatorHum");
            elevator.OpenElevator();
        }));
        waitingForPlayer = false;
    }

    //used to check if player has keycard to move on to next stage
    public bool InsertItem(ItemObject requirement)
    {
        if (waitingForPlayer)
        {
            return false;
        }
        bool a = elevator.InsertItem(requirement);
        if (a)
        {
            StartCoroutine(NextStage());
        }
        return a;
    }

    [MenuItem("Developer/Next Stage")]
    public static void NextStageDev()
    {
        instance.StartCoroutine(instance.NextStage());
    }

    void SpawnPlayer()
    {
        player.SetPositionAndRotation(playerSpawn.position, Quaternion.identity);

        SoundManager.instance.StopSound("Hum");
        SoundManager.instance.PlaySound("ElevatorHum");
        StartCoroutine(playerController.cameraController.Shake(shakeTime, shakeMag));
        playerController.SpawnFreeze();

        StartCoroutine(WaitAndExec(unlockPlayerTime, () =>
        {
            playerController.SpawnUnlock();
        }));
    }

    void SpawnEnemy()
    {
        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            enemyController.Spawn(oogaManSpawns[UnityEngine.Random.Range(0, oogaManSpawns.Length)].position);
        }));
    }

    IEnumerator WaitAndExec(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSeconds(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }
}

public class DeathArgs : EventArgs
{
    public int death;

    public DeathArgs(int i)
    {
        death = i;
    }
}

public class StageArgs : EventArgs
{
    public int stage;

    public StageArgs(int i)
    {
        stage = i;
    }
}