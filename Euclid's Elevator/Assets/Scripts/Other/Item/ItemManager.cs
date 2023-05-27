using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
struct SpawnerGroup
{
    public ItemSpawner[] spawners;
    public int activeStage;
}
[System.Serializable]
struct ItemSpawn
{
    public ItemObject item;
    public int probability;
}

[System.Serializable]
struct KeycardSpawn
{
    public Transform[] points;
    public int stage;
}

public class ItemManager : MonoBehaviour
{
    [SerializeField] SpawnerGroup[] spawnerGroups;
    [SerializeField] ItemSpawn[] spawns;
    [SerializeField] KeycardSpawn[] keySpawns;
    [SerializeField] ItemObject spawnOnDeath;
    [SerializeField] ItemObject keycard;

    private void Awake()
    {
        GameManager.Instance.OnDeath += (object caller, DeathArgs args) =>
        {
            SpawnItem(spawnOnDeath);
        };

        GameManager.Instance.OnStageStart += (object caller, StageArgs args) =>
        {
            if (args.stage != 4)
            {
                SpawnItems();
            }
            SpawnKeycard(args.stage);
        };
    }

    void SpawnItems()
    {
        List<ItemSpawner> spawners = new();

        foreach(SpawnerGroup gr in spawnerGroups)
        {
            if (gr.activeStage == GameManager.Instance.stage)
            {
                spawners.AddRange(gr.spawners);
            }
        }

        foreach (ItemSpawner spawner in spawners)
        {
            float probability = Random.Range(0, 100f);
            float current = -1;

            for(int i = 0; i < spawns.Length; i++)
            {
                current += spawns[i].probability;
                if (current >= probability)
                {
                    if (spawner.Spawn(spawns[i].item))
                    {
                        break;
                    }
                }
            }
        }
    }

    void SpawnItem(ItemObject item)
    {
        List<ItemSpawner> spawners = new();

        foreach (SpawnerGroup gr in spawnerGroups)
        {
            if (gr.activeStage == GameManager.Instance.stage)
            {
                spawners.AddRange(gr.spawners);
            }
        }

        int spawnerIndex = Random.Range(0, spawners.Count);

        spawners[spawnerIndex].ForceSpawn(item);
    }

    void SpawnKeycard(int stage)
    {
        foreach (KeycardSpawn spawn in keySpawns)
        {
            if (spawn.stage == stage)
            {
                int i = Random.Range(0, spawn.points.Length);
                Instantiate(keycard.prefab, spawn.points[i].position, Quaternion.identity);
                break;
            }
        }
    }
}
