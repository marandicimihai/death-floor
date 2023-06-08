using UnityEngine;

public class Player : MonoBehaviour
{
    public bool Dead { get; private set; }
    public FirstPersonController controller;
    public CameraController cameraController;
    public CameraAnimation cameraAnimation;
    public VFXManager vfxmanager;
    public Inventory inventory;
    public InventoryHUD inventoryHUD;
    public Lockpick lockpick;
    public InteractionManager interactionManager;
    [SerializeField] float spawnFreezeTime;
    [SerializeField] int maxDeaths;
    
    int deaths;

    public void Die(bool callDeath)
    {
        if (!Dead)
        {
            Dead = true;
            deaths++;

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
        GameManager.Instance.PlayerDeath(deaths, maxDeaths);
    }

    public void Spawn(Vector3 position)
    {
        Dead = false;
        controller.Spawn(position, spawnFreezeTime);
        cameraController.Spawn(spawnFreezeTime);
    }
}
