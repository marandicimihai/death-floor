using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public delegate void OnDeathDel(int deaths, int maxDeaths);
    public OnDeathDel PlayerDied;

    public delegate void OnSpawnDel(SpawnArgs args);
    public OnSpawnDel OnSpawn;
    public bool Dead { get; private set; }
    public int Deaths { get; private set; }

    public FirstPersonController controller;
    public CameraController cameraController;
    public CameraAnimation cameraAnimation;
    public VFXManager vfxmanager;
    public Inventory inventory;
    public Journal journal;
    public PlayerHUDManager HUDManager;
    public Lockpick lockpick;
    public InteractionManager interactionManager;
    public Insanity insanity;
    [SerializeField] float spawnFreezeTime;
    [SerializeField] int maxDeaths;

    private void Start()
    {
        if (SaveSystem.Instance.currentSaveData != null)
        {
            if (SaveSystem.Instance.currentSaveData.PlayerPosition.Length != 0)
            {
                transform.position = new Vector3(SaveSystem.Instance.currentSaveData.PlayerPosition[0],
                                                 SaveSystem.Instance.currentSaveData.PlayerPosition[1],
                                                 SaveSystem.Instance.currentSaveData.PlayerPosition[2]);
            }

            if (SaveSystem.Instance.currentSaveData.PlayerRotation.Length != 0)
            {
                transform.rotation = new Quaternion(SaveSystem.Instance.currentSaveData.PlayerRotation[0],
                                                    SaveSystem.Instance.currentSaveData.PlayerRotation[1],
                                                    SaveSystem.Instance.currentSaveData.PlayerRotation[2],
                                                    SaveSystem.Instance.currentSaveData.PlayerRotation[3]);
            }
        }

        SaveSystem.Instance.OnSaveGame += (ref GameData data) =>
        {
            data.PlayerPosition = new float[]
            {
                transform.position.x,
                transform.position.y,
                transform.position.z
            };
            data.PlayerRotation = new float[]
            {
                transform.rotation.x,
                transform.rotation.y,
                transform.rotation.z,
                transform.rotation.w
            };
            SaveSystem.Instance.CanSave = !Dead;
        };
    }

    public void Die(bool callDeath)
    {
        if (!Dead)
        {
            Dead = true;
            Deaths++;

            controller.Disable();
            cameraController.Disable();
            inventory.ClearInventory();
            if (callDeath)
            {
                CallDeath();
            }
        }
    }

    public void CallDeath()
    {
        PlayerDied?.Invoke(Deaths, maxDeaths);
    }

    public void Spawn(Vector3 position)
    {
        OnSpawn?.Invoke(new SpawnArgs(position, spawnFreezeTime));
        Dead = false;
    }
}

public class SpawnArgs : EventArgs
{
    public Vector3 position;
    public float freezeTime;

    public SpawnArgs(Vector3 position, float freezeTime)
    {
        this.position = position;
        this.freezeTime = freezeTime;
    }
}