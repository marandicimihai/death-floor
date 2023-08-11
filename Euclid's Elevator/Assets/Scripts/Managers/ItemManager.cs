using System.Collections.Generic;
using DeathFloor.SaveSystem;
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

public class ItemManager : MonoBehaviour, ISaveData<ItemData>
{
    [SerializeField] SpawnerGroup[] spawnerGroups;
    [SerializeField] ItemSpawn[] spawns;
    [SerializeField] KeycardSpawn[] keySpawns;
    [SerializeField] ItemProperties toolbox;
    [SerializeField] ItemProperties keycard;

    static List<Item> spawnedItems;

    public bool CanSave => true;

    private void Start()
    {
        spawnedItems = new();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnElevatorDoorClosed += (object caller, System.EventArgs args) =>
            {
                SpawnItems(GameManager.Instance.Stage);
                SpawnKeycard(GameManager.Instance.Stage);
            };

            GameManager.Instance.OnDeath += (object caller, System.EventArgs args) =>
            {
                if (!GameObject.FindGameObjectWithTag("Toolbox"))
                {
                    SpawnItem(toolbox, GameManager.Instance.Stage);
                }
                if (!GameObject.FindGameObjectWithTag("KeyCard") && GameManager.Instance.GameStage != GameStage.WaitForPlayer)
                {
                    SpawnKeycard(GameManager.Instance.Stage);
                }
            };
        }
        else
        {
            Debug.Log("No game manager.");
        }
    }

    public void OnFirstTimeLoaded()
    {
        SpawnKeycard(1);
    }

    public ItemData OnSaveData()
    {
        ItemProperties[] props = new ItemProperties[spawnedItems.Count];
        Vector3[] positions = new Vector3[spawnedItems.Count];
        string[][] vars = new string[spawnedItems.Count][];
        for (int i = 0; i < spawnedItems.Count; i++)
        {
            Item current = spawnedItems[i];
            props[i] = current.properties;
            positions[i] = current.transform.position;
            vars[i] = current.GetValues().ToArray();
        }

        return new ItemData(props, positions, vars);
    }

    public void LoadData(ItemData data)
    {
        ItemProperties[] props = data.ItemsProperties;
        Vector3[] positions = data.ItemPositions;
        string[][] vars = data.ItemVariables;
        for (int i = 0; i < props.Length; i++)
        {
            Item newItem = Instantiate(props[i].physicalObject, positions[i], Quaternion.identity).GetComponent<Item>();
            if (newItem.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
            newItem.LoadValues(vars[i]);
            AddToPhysicalItems(newItem);
        }
    }

    void SpawnItems(int stage)
    {
        List<ItemSpawner> spawners = new();

        foreach (SpawnerGroup gr in spawnerGroups)
        {
            if (gr.activeStage == stage)
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

    void SpawnItem(ItemProperties item, int stage)
    {
        List<ItemSpawner> spawners = new();

        foreach (SpawnerGroup gr in spawnerGroups)
        {
            if (gr.activeStage == stage)
            {
                spawners.AddRange(gr.spawners);
            }
        }

        int spawnerIndex = Random.Range(0, spawners.Count);

        foreach (ItemSpawner spawner in spawners)
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

    /// <summary>
    /// Adds the item passed in to a private array which will be saved using the save system. The list is shared amongst all item managers.
    /// </summary>
    /// <param name="toAdd">The item to add.</param>
    public static void AddToPhysicalItems(Item toAdd)
    {
        spawnedItems.Add(toAdd);
    }

    public static void RemoveFromPhysicalItems(Item toRemove)
    {
        if (spawnedItems.Contains(toRemove))
        {
            spawnedItems.Remove(toRemove);
        }
    }
}