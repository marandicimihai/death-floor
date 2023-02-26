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
    [SerializeField] Vector3 playerSpawnEulers;
    [SerializeField] Transform[] oogaManSpawns;

    [SerializeField] Animator elevator;

    [SerializeField] int maxDeaths;

    int deaths;


    private void Awake()
    {
        instance = this;
        SpawnPlayerAndEnemy();
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
            SpawnPlayerAndEnemy();
            if (deaths >= maxDeaths)
            {
                Application.Quit();
            }
        }));
    }

    void SpawnPlayerAndEnemy()
    {
        player.SetPositionAndRotation(playerSpawn.position, Quaternion.Euler(playerSpawnEulers));

        StartCoroutine(WaitAndExec(unlockPlayerTime, () =>
        {
            if (player.TryGetComponent(out FpsController controller))
            {
                controller.Respawn();
            }
        }));

        if (oogaManSpawns.Length > 0)
        {
            enemy.position = oogaManSpawns[UnityEngine.Random.Range(0, oogaManSpawns.Length)].position;
        }


        StartCoroutine(WaitAndExec(timeUntilOpenElevator, () =>
        {
            if (enemy.TryGetComponent(out Enemy enemyC))
            {
                enemyC.Respawn();
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
