using System.Collections;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;
    public Transform enemy;

    [SerializeField] float timeAfterDeath;
    [SerializeField] float timeUntilOpenElevator;
    [SerializeField] float unlockPlayerTime;
    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform[] oogaManSpawns;

    [SerializeField] Animator elevator;

    [SerializeField] int maxDeaths;

    public EventHandler OnSpawn;
    public EventHandler<DeathArgs> OnDeath;

    int deaths;


    private void Awake()
    {
        instance = this;
        SpawnPlayerAndEnemy();
    }

    private void Start()
    {
        OnSpawn?.Invoke(this, new EventArgs());
    }

    public void Die()
    {
        elevator.SetBool("Open", false);
        if (enemy.TryGetComponent(out Enemy enemyC))
        {
            enemyC.Stop();
        }
        StartCoroutine(WaitAndExec(timeAfterDeath, () =>
        {
            deaths++;
            OnDeath?.Invoke(this, new DeathArgs(deaths));
            SpawnPlayerAndEnemy();
            if (deaths >= maxDeaths)
            {
                Debug.Log("GAMAJ OVAH");
            }
        }));
    }

    void SpawnPlayerAndEnemy()
    {
        player.SetPositionAndRotation(playerSpawn.position, Quaternion.identity);

        StartCoroutine(WaitAndExec(unlockPlayerTime, () =>
        {
            if (player.TryGetComponent(out FpsController controller))
            {
                controller.Respawn();
            }
        }));

        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            if (enemy.TryGetComponent(out Enemy enemyC))
            {
                enemyC.Respawn(oogaManSpawns[UnityEngine.Random.Range(0, oogaManSpawns.Length)].position);
            }
            elevator.SetBool("Open", true);
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