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

public class ItemManager : MonoBehaviour
{
    [SerializeField] SpawnerGroup[] spawnerGroups;
    [SerializeField] ItemSpawn[] spawns;
    [SerializeField] ItemObject spawnOnDeath;
    [SerializeField] ItemObject spawnOnStage;

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
            SpawnItem(spawnOnStage);
        };
    }

    public void SpawnItems()
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

    public void SpawnItem(ItemObject item)
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
}
