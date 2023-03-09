using System.Collections;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;
    public FpsController playerController;
    public Transform enemy;
    public Enemy enemyController;
    public bool elevatorOpen;

    [SerializeField] float timeAfterDeath;
    [SerializeField] float timeUntilOpenElevator;
    [SerializeField] float unlockPlayerTime;
    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform[] oogaManSpawns;

    [SerializeField] Animator elevator;

    [SerializeField] int maxDeaths;

    public EventHandler OnSpawn;
    public EventHandler<DeathArgs> OnDeath;
    public EventHandler OnEnd;

    int deaths;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SpawnPlayerAndEnemy();
        OnSpawn?.Invoke(this, new EventArgs());
    }

    public void Die()
    {
        elevator.SetBool("Open", false);
        elevatorOpen = false;
        enemyController.Stop();
        StartCoroutine(WaitAndExec(timeAfterDeath, () =>
        {
            deaths++;
            OnDeath?.Invoke(this, new DeathArgs(deaths));
            SpawnPlayerAndEnemy();
            if (deaths >= maxDeaths)
            {
                OnEnd?.Invoke(this, new EventArgs());
                Debug.Log("GAMAJ OVAH");
            }
        }));
    }

    void SpawnPlayerAndEnemy()
    {
        player.SetPositionAndRotation(playerSpawn.position, Quaternion.identity);

        playerController.SpawnFreeze();

        StartCoroutine(WaitAndExec(unlockPlayerTime, () =>
        {
            playerController.SpawnUnlock();
        }));

        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            enemyController.Respawn(oogaManSpawns[UnityEngine.Random.Range(0, oogaManSpawns.Length)].position);
            elevator.SetBool("Open", true);
            elevatorOpen = true;
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