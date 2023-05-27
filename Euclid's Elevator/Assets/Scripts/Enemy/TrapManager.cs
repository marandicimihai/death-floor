using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TrapSpawn
{
    public int stage;
    public Transform[] spawnPoints;
    public int count;
}

public class TrapManager : MonoBehaviour
{
    [SerializeField] TrapSpawn[] spawns;
    [SerializeField] GameObject trapPrefab;

    private void Start()
    {
        GameManager.Instance.OnStageStart += (object caller, StageArgs args) =>
        {
            SpawnTraps(args.stage);
        };
    }

    void SpawnTraps(int stage)
    {
        EnemyTrap[] traps = FindObjectsOfType<EnemyTrap>();

        foreach(EnemyTrap tr in traps)
        {
            Destroy(tr.gameObject);
        }

        foreach (TrapSpawn spawn in spawns)
        {
            if (spawn.stage == stage)
            {
                List<Transform> used = new();
                for (int i = 0; i < spawn.count; i++)
                {
                    int point = Random.Range(0, spawn.spawnPoints.Length);

                    if (used.Contains(spawn.spawnPoints[point]))
                    {
                        bool gate = false;
                        for (int j = 0; j < spawn.count; j++)
                        {
                            point = (point + 1) % spawn.spawnPoints.Length;

                            if (!used.Contains(spawn.spawnPoints[point]))
                            {
                                Instantiate(trapPrefab, spawn.spawnPoints[point].position, Quaternion.identity);
                                used.Add(spawn.spawnPoints[point]);
                                gate = true;
                            }
                        }
                        if (!gate)
                        {
                            Debug.Log("More traps than spawn points");
                        }
                    }
                    else
                    {
                        Instantiate(trapPrefab, spawn.spawnPoints[point].position, Quaternion.identity);
                        used.Add(spawn.spawnPoints[point]);
                    }
                }
            }
        }
    }
}
