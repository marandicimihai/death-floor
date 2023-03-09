using UnityEngine;

public class Pills : Item
{
    [SerializeField] float insanityDecrease;

    public override void UseItem(FpsController controller)
    {
        controller.insanity.InsanityMeter -= insanityDecrease;
    }
}