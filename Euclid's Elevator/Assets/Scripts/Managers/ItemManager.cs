using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
struct SpawnerGroup
{
    public ItemSpawner[] spawners;
    public int activeStage;
}
[System.Serializable]
struct ItemSpawn
{
    public ItemProperties item;
    public int probability;
}

[System.Serializable]
struct KeycardSpawn
{
    public ItemSpawner[] points;
    public int stage;
}

public class ItemManager : MonoBehaviour
{
    public static List<Item> spawnedItems;

    [SerializeField] ItemProperties[] scriptableobjects;
    [SerializeField] SpawnerGroup[] spawnerGroups;
    [SerializeField] ItemSpawn[] spawns;
    [SerializeField] KeycardSpawn[] keySpawns;
    [SerializeField] ItemProperties toolbox;
    [SerializeField] ItemProperties keycard;

    private void Start()
    {
        spawnedItems = new();
        
        GameManager.Instance.OnStageStart += (object caller, System.EventArgs args) =>
        {
            SpawnItems();
            SpawnKeycard(GameManager.Instance.Stage);
        };

        GameManager.Instance.OnDeath += (object caller, System.EventArgs args) =>
        {
            if (!GameObject.FindGameObjectWithTag("Toolbox"))
            {
                SpawnItem(toolbox);
            }
            if (!GameObject.FindGameObjectWithTag("KeyCard") && GameManager.Instance.GameStage != GameStage.WaitForPlayer)
            {
                SpawnKeycard(GameManager.Instance.Stage);
            }
        };

        SpawnKeycard(1);
    }

    ItemProperties GetProperties(string name)
    {
        foreach (ItemProperties prop in scriptableobjects)
        {
            if (prop.name == name)
            {
                return prop;
            }
        }
        return null;
    }

    void SpawnItems()
    {
        List<ItemSpawner> spawners = new();

        foreach (SpawnerGroup gr in spawnerGroups)
        {
            if (gr.activeStage == GameManager.Instance.Stage)
            {
                spawners.AddRange(gr.spawners);
            }
        }

        foreach (ItemSpawner spawner in spawners)
        {
            float probability = Random.Range(0, 100f);
            float current = -1;

            for (int i = 0; i < spawns.Length; i++)
            {
                current += spawns[i].probability;
                if (current >= probability)
                {
                    if (spawner.Spawn(spawns[i].item, out Item spawned))
                    {
                        spawnedItems.Add(spawned);
                        break;
                    }
                }
            }
        }
    }

    void SpawnItem(ItemProperties item)
    {
        List<ItemSpawner> spawners = new();

        foreach (SpawnerGroup gr in spawnerGroups)
        {
            if (gr.activeStage == GameManager.Instance.Stage)
            {
                spawners.AddRange(gr.spawners);
            }
        }

        int spawnerIndex = Random.Range(0, spawners.Count);

        foreach(ItemSpawner spawner in spawners)
        {
            if (spawner.Spawn(item, out Item spawned))
            {
                spawnedItems.Add(spawned);
                return;
            }
            spawnerIndex = (spawnerIndex + 1) % spawners.Count;
        }

        spawnerIndex = Random.Range(0, spawners.Count);
        spawners[spawnerIndex].ForceSpawn(item, out Item spawned2);
        spawnedItems.Add(spawned2);
    }

    void SpawnKeycard(int stage)
    {
        foreach (KeycardSpawn spawn in keySpawns)
        {
            if (spawn.stage == stage)
            {
                int spawnerIndex = Random.Range(0, spawn.points.Length);

                foreach (ItemSpawner spawner in spawn.points)
                {
                    if (spawner.Spawn(keycard, out Item spawned))
                    {
                        spawnedItems.Add(spawned);
                        return;
                    }
                    spawnerIndex = (spawnerIndex + 1) % spawn.points.Length;
                }

                spawnerIndex = Random.Range(0, spawn.points.Length);
                spawn.points[spawnerIndex].ForceSpawn(keycard, out Item spawned2);
                spawnedItems.Add(spawned2);
                return;
            }
        }
    }
}