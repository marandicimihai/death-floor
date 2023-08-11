using System.Collections.Generic;
using DeathFloor.SaveSystem;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct TrapSpawn
{
    public int stage;
    public Transform[] spawnPoints;
    public int count;
}

public class TrapManager : MonoBehaviour, ISaveData<TrapData>
{
    public static List<Transform> spawnedtraps;

    public bool CanSave => true;

    [SerializeField] TrapSpawn[] spawns;
    [SerializeField] GameObject trapPrefab;

    private void Start()
    {
        GameManager.Instance.OnElevatorDoorClosed += (object caller, System.EventArgs args) =>
        {
            SpawnTraps(GameManager.Instance.Stage);
        };

        spawnedtraps = new();
    }

    public void OnFirstTimeLoaded()
    {

    }

    public TrapData OnSaveData()
    {
        Vector3[] positions = new Vector3[spawnedtraps.Count];

        for (int i = 0; i < spawnedtraps.Count; i++)
        {
            positions[i] = spawnedtraps[i].position;
        }

        return new TrapData(positions);
    }

    public void LoadData(TrapData data)
    {
        Vector3[] positions = data.TrapPositions;

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject newTrap = Instantiate(trapPrefab, positions[i], Quaternion.identity);
            spawnedtraps.Add(newTrap.transform);
        }
    }

    void SpawnTraps(int stage)
    {
        foreach (Transform tr in spawnedtraps)
        {
            Destroy(tr.gameObject);
        }

        spawnedtraps = new();

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