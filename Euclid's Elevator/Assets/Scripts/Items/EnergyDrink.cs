using UnityEngine;

public class EnergyDrink : Item
{
    [SerializeField] float speedMultiplier;
    [SerializeField] float lockPickMultiplier;
    [SerializeField] float effectTime;

    public override void UseItem(FpsController controller)
    {
        controller.IncreaseSpeed(speedMultiplier, effectTime);

        if (controller.TryGetComponent(out Lockpick pick))
            pick.BoostLockPick(lockPickMultiplier, effectTime);
    }
}
