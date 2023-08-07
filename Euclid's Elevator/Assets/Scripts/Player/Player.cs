using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void OnDeathDel(int deaths, int maxDeaths);
    public OnDeathDel PlayerDied;

    public bool Dead { get; private set; }
    public int Deaths { get; private set; }

    [SerializeField] FirstPersonController controller;
    [SerializeField] CameraController cameraController;
    [SerializeField] VFXManager vfxmanager;
    [SerializeField] Inventory inventory;
    [SerializeField] Insanity insanity;
    [SerializeField] Journal journal;
    [SerializeField] PlayerHUDManager HUDManager;
    [SerializeField] float spawnFreezeTime;
    [SerializeField] int maxDeaths;

    private void Start()
    {
        if (SaveSystem.Instance != null)
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
        else
        {
            Debug.Log("No save system");
        }
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