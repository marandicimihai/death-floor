using UnityEngine;

public class Player : MonoBehaviour
{
    public FirstPersonController controller;
    public CameraController cameraController;
    public Inventory inventory;
    [SerializeField] float spawnFreezeTime;
    [SerializeField] int maxDeaths;
    
    int deaths;

    public void Die(float respawnTime)
    {
        deaths++;

        controller.Disable();
        cameraController.Disable();
        inventory.ClearInventory();
        Invoke(nameof(CallDeath), respawnTime);
    }

    void CallDeath()
    {
        GameManager.Instance.PlayerDeath(deaths, maxDeaths);
    }

    public void Spawn(Vector3 position)
    {
        controller.Spawn(position, spawnFreezeTime);
        cameraController.Spawn(spawnFreezeTime);
    }
}
