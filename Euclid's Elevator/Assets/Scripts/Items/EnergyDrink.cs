using UnityEngine;

public class EnergyDrink : Item
{
    [SerializeField] float speedMultiplier;
    [SerializeField] float effectTime;

    public override void UseItem(FpsController controller)
    {
        controller.IncreaseSpeed(speedMultiplier, effectTime);
    }
}
