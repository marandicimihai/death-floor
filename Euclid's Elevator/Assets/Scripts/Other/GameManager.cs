using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
struct EnemySpawn
{
    public Transform spawn;
    public int stage;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool Paused { get; private set; }
    static List<MonoBehaviour> pausable;

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
    [SerializeField] EnemySpawn[] oogaManSpawns;
    [SerializeField] float shakeTime;
    [SerializeField] float shakeMag;

    [Header("Game Settings")]
    [SerializeField] int stages;

    public event EventHandler<StageArgs> OnStageStart;
    public event EventHandler OnElevatorDoorClosed;
    
    public EventHandler OnSpawn;
    public EventHandler<DeathArgs> OnDeath;
    public EventHandler OnEnd;

    public EventHandler<PauseArgs> OnPause;
    public EventHandler OnUnpause;

    bool waitingForPlayer;

    public int stage;
    int deaths;

    private void Awake()
    {
        stage = 1;
        Instance = this;
        pausable = new List<MonoBehaviour>();
    }

    private void Start()
    {
        playerController.PlayerInputActions.General.Pause.performed += TogglePause;
        Settings set = SaveSystem.LoadSettings();

        if (set != null)
        {
            SoundManager.Instance.SetAmbianceVolume(set.ambianceVolume);
            SoundManager.Instance.SetEffectsVolume(set.effectsVolume);
            playerController.cameraController.SetBloom(set.bloom);
            playerController.cameraController.SetBlur(set.blur);
            playerController.cameraController.SetSens(set.sensitivity);
        }

        StartGame();
    }

    #region GameLoop

    //called once
    void StartGame()
    {
        stage = 1;
        OnStageStart?.Invoke(this, new StageArgs(stage));

        OnSpawn?.Invoke(this, new EventArgs());

        SpawnPlayer();
        SpawnEnemy();
        InitElevator();
    }

    //respawns player and enemy
    public void PlayerDied()
    {
        elevator.CloseElevator();
        elevator.BreakDown();
        enemyController.Stop(timeAfterDeath);

        StartCoroutine(WaitAndExec(timeAfterDeath, () =>
        {
            deaths++;
            if (deaths >= maxDeaths)
            {
                OnEnd?.Invoke(this, new EventArgs());
                Debug.Log("Game Over! U ded!");
            }
            else
            {
                OnDeath?.Invoke(this, new DeathArgs(deaths));
                SpawnPlayer();
                SpawnEnemy();
                InitElevator();
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
        SoundManager.Instance.PlaySound("ElevatorHum");
        SoundManager.Instance.StopSound("Hum");
        StartCoroutine(playerController.cameraController.Shake(shakeTime, shakeMag));

        if (stage >= stages )
        {
            Debug.Log("You won!");
        }
        else
        {
            stage++;
            OnStageStart?.Invoke(this, new StageArgs(stage));
        }

        StartCoroutine(WaitAndExec(1.5f, () => SpawnEnemy()));
        InitElevator();
        waitingForPlayer = false;
    }

    #endregion

    #region Elevator
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

    public void ElevatorDoorClosed()
    {
        OnElevatorDoorClosed?.Invoke(this, new EventArgs());
    }

    #endregion

    #region Other
    /*[MenuItem("Developer/Next Stage")]
    public static void NextStageDev()
    {
        Instance.StartCoroutine(Instance.NextStage());
    }*/
    
    void SpawnPlayer()
    {
        player.position = playerSpawn.position;

        SoundManager.Instance.StopSound("Hum");
        SoundManager.Instance.PlaySound("ElevatorHum");
        StartCoroutine(playerController.cameraController.Shake(shakeTime, shakeMag));
        playerController.SpawnFreeze();

        StartCoroutine(WaitAndExec(unlockPlayerTime, () =>
        {
            playerController.SpawnUnlock();
        }));
    }

    void SpawnEnemy()
    {
        List<EnemySpawn> spawns = new List<EnemySpawn>();
        if (stage == 4)
        {
            foreach (EnemySpawn spawn in oogaManSpawns)
            {
                spawns.Add(spawn);
            }
        }
        else
        {
            foreach (EnemySpawn spawn in oogaManSpawns)
            {
                if (spawn.stage == stage)
                {
                    spawns.Add(spawn);
                }
            }
        }

        enemyController.Reset(spawns[UnityEngine.Random.Range(0, spawns.Count)].spawn.position, 1);
    }

    void InitElevator()
    {
        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            SoundManager.Instance.PlaySound("ElevatorStop");
            SoundManager.Instance.StopSound("ElevatorHum");
            elevator.OpenElevator();
        }));
    }

    #endregion

    #region Pause

    /*[MenuItem("Developer/Pause")]
    public static void PauseDev()
    {
        Instance.Pause();
    }

    [MenuItem("Developer/Unpause")]
    public static void UnpauseDev()
    {
        Instance.Unpause();
    }*/

    /// <summary>
    /// Use method in OnEnable() or later
    /// </summary>
    /// <param name="behaviour"></param>
    public static void MakePausable(MonoBehaviour beh)
    {
        pausable.Add(beh);
    }

    public static void MakeUnpausable(MonoBehaviour beh)
    {
        pausable.Remove(beh);
    }

    void TogglePause(InputAction.CallbackContext context)
    {
        if (Paused)
        {
            Unpause();
        }
        else
        {
            Pause(true);
        }
    }


    /// <summary>
    /// Pauses game
    /// </summary>
    /// <param name="UI">Specifies whether the UI should be turned off</param>
    public void Pause(bool UI = false)
    {
        OnPause?.Invoke(this, new PauseArgs(UI));
        Paused = true;
        foreach (MonoBehaviour beh in pausable)
        {
            beh.enabled = false;
        }
        foreach(AudioSource s in FindObjectsOfType<AudioSource>())
        {
            if (!SoundManager.Instance.realtime.Contains(s))
            {
                s.Pause();
            }
        }
        if (!playerController.journal.InJournalView)
        {
            playerController.journal.CancelExitCall();
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        OnUnpause?.Invoke(this, new EventArgs());
        Paused = false;
        foreach (MonoBehaviour beh in pausable)
        {
            beh.enabled = true;
        }
        foreach (AudioSource s in FindObjectsOfType<AudioSource>())
        {
            s.UnPause();
        }
        StartCoroutine(playerController.journal.CallExitWhenAvailable());
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    #endregion

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

#region EventArgs

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

public class PauseArgs : EventArgs
{
    public bool UI;

    public PauseArgs(bool a)
    {
        UI = a;
    }
}

#endregion