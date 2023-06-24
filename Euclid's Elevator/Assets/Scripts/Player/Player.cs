using UnityEngine;

public class Player : MonoBehaviour
{
    public bool Dead { get; private set; }
    public int Deaths { get; private set; }

    public FirstPersonController controller;
    public CameraController cameraController;
    public CameraAnimation cameraAnimation;
    public VFXManager vfxmanager;
    public Inventory inventory;
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
        Dead = false;
        controller.Spawn(position, spawnFreezeTime);
        cameraController.Spawn(spawnFreezeTime);
    }
}
