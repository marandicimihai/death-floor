using UnityEngine;

public class Pills : Item
{
    [SerializeField] float insanityDecrease;

    public override void UseItem(FpsController controller)
    {
        if (controller.TryGetComponent(out Insanity insanity))
        {
            insanity.InsanityMeter -= insanityDecrease;
        }
    }
}