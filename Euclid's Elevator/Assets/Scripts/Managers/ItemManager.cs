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
    [SerializeField] ItemProperties[] scriptableobjects;
    [SerializeField] SpawnerGroup[] spawnerGroups;
    [SerializeField] ItemSpawn[] spawns;
    [SerializeField] KeycardSpawn[] keySpawns;
    [SerializeField] ItemProperties toolbox;
    [SerializeField] ItemProperties keycard;

    static List<Item> spawnedItems;

    private void Start()
    {
        spawnedItems = new();
        if (SaveSystem.Instance != null)
        {
            if (SaveSystem.Instance.currentSaveData != null &&
                SaveSystem.Instance.currentSaveData.spawneditems.Length != 0)
            {
                int i = 0;
                List<float> positions = SaveSystem.Instance.currentSaveData.spawneditemPositions.ToList();
                List<string> variables = SaveSystem.Instance.currentSaveData.spawnedvariables.ToList();
                foreach (string itemName in SaveSystem.Instance.currentSaveData.spawneditems)
                {
                    Vector3 position = new Vector3(positions[i * 3],
                                                   positions[i * 3 + 1],
                                                   positions[i * 3 + 2]);

                    Item newItem = Instantiate(GetProperties(SaveSystem.Instance.currentSaveData.spawneditems[i]).physicalObject, position, Quaternion.identity).GetComponent<Item>();

                    if (newItem.TryGetComponent(out Rigidbody rb))
                    {
                        rb.isKinematic = true;
                    }

                    List<string> currentvars = new();

                    for (int j = 0; j < SaveSystem.Instance.currentSaveData.spawnedlengths[i]; j++)
                    {
                        currentvars.Add(variables[0]);
                        variables.Remove(variables[0]);
                    }

                    newItem.LoadValues(currentvars.ToArray());
                    spawnedItems.Add(newItem);
                    i++;
                }
            }

            if (SaveSystem.Instance.currentSaveData != null && SaveSystem.Instance.currentSaveData.stage < 0)
            {
                SpawnKeycard(1);
            }
            else if (SaveSystem.Instance.currentSaveData == null)
            {
                SpawnKeycard(1);
            }
            SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
            {
                List<string> spawnedItemNames = new();
                List<float> spawnedItemPositions = new();
                List<int> spawnedItemLengths = new();
                List<string> spawnedItemVariables = new();

                foreach (Item item in spawnedItems)
                {
                    spawnedItemNames.Add(item.properties.name);
                    spawnedItemPositions.Add(item.transform.position.x);
                    spawnedItemPositions.Add(item.transform.position.y);
                    spawnedItemPositions.Add(item.transform.position.z);
                    foreach (object obj in item.GetSaveVariables())
                    {
                        spawnedItemVariables.Add(obj.ToString());
                    }
                    spawnedItemLengths.Add(item.GetSaveVariables().Count);
                }

                data.spawneditems = spawnedItemNames.ToArray();
                data.spawneditemPositions = spawnedItemPositions.ToArray();
                data.spawnedlengths = spawnedItemLengths.ToArray();
                data.spawnedvariables = spawnedItemVariables.ToArray();
            };
        }
        else
        {
            Debug.Log("No save system.");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStageStart += (object caller, System.EventArgs args) =>
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