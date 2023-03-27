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
    [SerializeField] Elevator elevator;

    [Header("Spawn Settings")]
    [SerializeField] int maxDeaths;
    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform[] oogaManSpawns;
    [SerializeField] float shakeTime;
    [SerializeField] float shakeMag;

    [Header("Game Settings")]
    public bool spawnEnemy;
    [SerializeField] int stages;
    [SerializeField] ItemObject[] stageRequirements;

    public event EventHandler<StageArgs> OnStageStart;
    
    public EventHandler OnSpawn;
    public EventHandler<DeathArgs> OnDeath;
    public EventHandler OnEnd;

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

    private void OnValidate()
    {
        if (!spawnEnemy)
        {
            enemy.gameObject.SetActive(false);
        }
        else
        {
            enemy.gameObject.SetActive(true);
        }
    }

    //called once
    void StartGame()
    {
        if (stageRequirements.Length > stages)
        {
            stageRequirements = new ItemObject[]
            {
                stageRequirements[0],
                stageRequirements[1],
                stageRequirements[2],
                stageRequirements[3]
            };
        }

        SpawnPlayer();
        SpawnEnemy();
        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
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
        if (enemyController != null)
            enemyController.Stop();

        StartCoroutine(WaitAndExec(timeAfterDeath, () =>
        {
            deaths++;
            if (deaths >= maxDeaths)
            {
                OnEnd?.Invoke(this, new EventArgs());
                Debug.Log("GAMAJ OVAH");
            }
            else
            {
                OnDeath?.Invoke(this, new DeathArgs(deaths));
                SpawnPlayer();
                SpawnEnemy();
                StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
                {
                    elevator.OpenElevator();
                }));
            }
        }));
    }

    void SpawnPlayer()
    {
        player.SetPositionAndRotation(playerSpawn.position, Quaternion.identity);

        StartCoroutine(playerController.cameraController.Shake(shakeTime, shakeMag));
        playerController.SpawnFreeze();

        StartCoroutine(WaitAndExec(unlockPlayerTime, () =>
        {
            playerController.SpawnUnlock();
        }));
    }

    void SpawnEnemy()
    {
        if (!spawnEnemy)
            return;

        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            enemyController.Respawn(oogaManSpawns[UnityEngine.Random.Range(0, oogaManSpawns.Length)].position);
        }));
    }

    [MenuItem("Developer/Next Stage")]
    public static void NextStageDev()
    {
        instance.StartCoroutine(instance.NextStage());
    }
    //moves on to next stage
    IEnumerator NextStage()
    {
        while (Vector3.Distance(elevatorPoint, player.position) > elevatorRadius)
        {
            yield return null;
        }

        elevator.CloseElevator();
        StartCoroutine(playerController.cameraController.Shake(shakeTime, shakeMag));

        stage++;
        OnStageStart?.Invoke(this, new StageArgs(stage));

        SpawnEnemy();
        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            elevator.OpenElevator();
        }));
    }
    //used to check if player ahs keycard to move on to next stage
    public bool InsertItem(ItemObject requirement)
    {
        if (requirement.name == stageRequirements[stage - 1].name)
        {
            StartCoroutine(NextStage());
            return true;
        }
        return false;
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