using UnityEngine;
using System;

public class Player : MonoBehaviour
{
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
        GameManager.Instance.PlayerDeath(Deaths, maxDeaths);
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