using DeathFloor.SaveSystem;
using UnityEngine;

public class Player : MonoBehaviour, ISaveData<PlayerData>
{
    public delegate void OnDeathDel(int deaths, int maxDeaths);
    public OnDeathDel PlayerDied;

    public bool Dead { get; private set; }
    public int Deaths { get; private set; }

    public bool CanSave => !Dead;

    [SerializeField] FirstPersonController controller;
    [SerializeField] CameraController cameraController;
    [SerializeField] VFXManager vfxmanager;
    [SerializeField] Inventory inventory;
    [SerializeField] Insanity insanity;
    [SerializeField] Journal journal;
    [SerializeField] PlayerHUDManager HUDManager;
    [SerializeField] float spawnFreezeTime;
    [SerializeField] int maxDeaths;

    public void OnFirstTimeLoaded()
    {
        
    }

    public PlayerData OnSaveData()
    {
        return new PlayerData(transform.position, transform.rotation);
    }

    public void LoadData(PlayerData data)
    {
        transform.SetPositionAndRotation(data.PlayerPosition, data.PlayerRotation);
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
            insanity.ReduceSanity(1);
            journal.CloseJournal();

            Input.Instance.InputActions.Disable();

            if (callDeath)
            {
                CallDeath();
            }
        }
        else
        {
            Debug.Log("Called death twice!");
        }
    }

    public void CallDeath()
    {
        //ONLY GAME MANAGER USES IT ATM
        PlayerDied?.Invoke(Deaths, maxDeaths);
    }

    public void Spawn(Vector3 position)
    {
        Input.Instance.InputActions.Enable();

        Freeze();
        transform.position = position;

        Invoke(nameof(UnFreeze), spawnFreezeTime);

        vfxmanager.ResetEffects();
        Dead = false;
    }

    public void Freeze()
    {
        controller.Disable();
        cameraController.Disable();
    }

    public void UnFreeze()
    {
        controller.Enable();
        cameraController.Enable();
    }

}