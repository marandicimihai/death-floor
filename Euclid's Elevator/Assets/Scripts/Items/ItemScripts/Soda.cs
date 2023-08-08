using UnityEngine;

public class Soda : Item, IUsable
{
    [SerializeField] [SyncValue] float speedBoost;
    [SerializeField] [SyncValue] float lockpickBoost;
    [SerializeField] [SyncValue] float time;

    FirstPersonController controller;

    protected override void Start()
    {
        base.Start();
        controller = FindObjectOfType<FirstPersonController>();
    }

    public bool OnUse()
    {
        if (controller != null)
        {
            controller.BoostForTime(speedBoost, time);
        }
        else
        {
            Debug.Log("No controller.");
        }
        //player.lockpick.BoostForTime(lockpickBoost, time);
        return true;
    }
}
