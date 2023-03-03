using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
struct ItemSpawn
{
    public ItemObject item;
    public int priority;
    [Range(0, 100)]
    public float probability;
}

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemSpawner[] spawners;
    [SerializeField] ItemSpawn[] spawns;

    private void Awake()
    {
        SpawnItems();
    }

    private void OnValidate()
    {
        if (spawns.Length <= 1)
            return;

        SortPriority();
        int i = spawns[0].priority, from = 0, index = 0;
        foreach(ItemSpawn s in spawns)
        {
            if (i != s.priority)
            {
                Clamp(from, index);
                from = index;
                i = s.priority;
            }
            index++;
        }
        Clamp(from, index);
    }

    public void SpawnItems()
    {
        if (spawns.Length <= 1)
            return;

        SortPriority();
        int i = spawns[0].priority, from = 0, index = 0;
        foreach (ItemSpawn s in spawns)
        {
            if (i != s.priority)
            {
                SpawnItemsOfPriority(from, index);
                from = index;
                i = s.priority;
            }
            index++;
        }
        SpawnItemsOfPriority(from, index);
    }

    void SpawnItemsOfPriority(int from, int to)
    {
        if (from == to)
            return;

        //try to spawn every item at most
        for (int i = from; i < to; i++)
        {
            int itemIndex = -1;

            //calculate item index by probability
            float prob = Random.Range(1f, 100);
            float sum = 0;
            for (int j = from; j < to; j++)
            {
                sum += spawns[j].probability;
                if (sum >= prob)
                {
                    itemIndex = j;
                    break;
                }
            }

            if (itemIndex == -1)
                continue;

            //choose spawner
            int spawnerIndex = Random.Range(0, spawners.Length);

            bool gate = false;
            for (int j = 0; j < spawners.Length; j++)
            {
                if (!spawners[spawnerIndex].hasItem && spawners[spawnerIndex].Spawn(spawns[itemIndex].item))
                {
                    gate = true;
                    break;
                }

                spawnerIndex = (spawnerIndex + 1) % (spawners.Length);
            }

            if (!gate)
            {
                Debug.Log($"Couldn't find spawner for {spawns[itemIndex].item.name}");
            }
        }
    }

    void SortPriority()
    {
        if (spawns.Length <= 1)
            return;

        for (int i = 0; i < spawns.Length - 1; i++)
        {
            bool a = true;
            for (int j = 1; j < spawns.Length; j++)
            {
                if (spawns[j - 1].priority > spawns[j].priority)
                {
                    ItemSpawn temp = spawns[j];
                    spawns[j] = spawns[j - 1];
                    spawns[j - 1] = temp;
                    a = false;
                }
            }
            if (a)
                return;
        }
    }

    void Clamp(int from, int to)
    {
        float sum = 0;

        for (int i = from; i < to; i++)
        {
            sum += spawns[i].probability;
        }

        if (sum <= 100)
            return;

        float surplus = sum - 100;

        for (int i = from; i < to; i++)
        {
            spawns[i].probability = spawns[i].probability - surplus * (spawns[i].probability / 100);
            spawns[i].probability = Mathf.Clamp(spawns[i].probability, 0, 100);
        }
    }
}
