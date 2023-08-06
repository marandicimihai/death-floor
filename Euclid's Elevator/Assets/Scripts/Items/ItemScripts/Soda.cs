using UnityEngine;

public class Soda : Item, IUsable
{
    [SerializeField] [SyncValue] float speedBoost;
    [SerializeField] [SyncValue] float lockpickBoost;
    [SerializeField] [SyncValue] float time;

    public bool OnUse(IBehaviourService service)
    {
        if (service.RequestComponentOfType(out FirstPersonController controller))
        {
            controller.BoostForTime(speedBoost, time);
        }
        player.lockpick.BoostForTime(lockpickBoost, time);
        return true;
    }
}
