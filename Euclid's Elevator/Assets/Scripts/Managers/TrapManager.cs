using System.Collections.Generic;
using System.Linq;
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
    public static List<Transform> spawnedtraps;

    [SerializeField] TrapSpawn[] spawns;
    [SerializeField] GameObject trapPrefab;

    private void Start()
    {
        GameManager.Instance.OnElevatorDoorClosed += (object caller, System.EventArgs args) =>
        {
            SpawnTraps(GameManager.Instance.Stage);
        };

        spawnedtraps = new();

        if (SaveSystem.CurrentSaveData != null)
        {
            if (SaveSystem.CurrentSaveData.trapCount > 0 &&
                SaveSystem.CurrentSaveData.trapPositions.Length > 0)
            {
                List<float> positions = SaveSystem.CurrentSaveData.trapPositions.ToList();
                for (int i = 0; i < SaveSystem.CurrentSaveData.trapCount; i++)
                {
                    Vector3 position = new(positions[i * 3], positions[i * 3 + 1], positions[i * 3 + 2]);

                    GameObject newTrap = Instantiate(trapPrefab, position, Quaternion.identity);
                    spawnedtraps.Add(newTrap.transform);
                }
            }
        }

        SaveSystem.OnSaveGame += (ref GameData data) =>
        {
            List<float> positions = new();

            foreach (Transform trap in spawnedtraps)
            {
                positions.Add(trap.transform.position.x);
                positions.Add(trap.transform.position.y);
                positions.Add(trap.transform.position.z);
            }

            data.trapCount = spawnedtraps.Count;
            data.trapPositions = positions.ToArray();
        };
    }

    void SpawnTraps(int stage)
    {
        foreach (Transform tr in spawnedtraps)
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
                        for (int j = 0; j < spawn.count; j++)
                        {
                            point = (point + 1) % spawn.spawnPoints.Length;

                            if (!used.Contains(spawn.spawnPoints[point]))
                            {
                                GameObject newTrap = Instantiate(trapPrefab, spawn.spawnPoints[point].position, Quaternion.identity);
                                spawnedtraps.Add(newTrap.transform);
                                used.Add(spawn.spawnPoints[point]);
                                break;
                            }
                        }
                    }
                    else
                    {
                        spawnedtraps.Add(Instantiate(trapPrefab, spawn.spawnPoints[point].position, Quaternion.identity).transform);
                        used.Add(spawn.spawnPoints[point]);
                    }
                }
            }
        }
    }

    public static void RemoveFromTraps(Transform tr)
    {
        if (spawnedtraps.Contains(tr))
        {
            spawnedtraps.Remove(tr);
        }
    }
}