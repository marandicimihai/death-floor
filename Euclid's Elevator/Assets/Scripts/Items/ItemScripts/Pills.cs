using UnityEngine;

public class Pills : Item, IUsable
{
    [SerializeField] [SyncValue] float sanityDecreasePercentage;

    public bool OnUse(IBehaviourService service)
    {
        if (service.RequestComponentOfType(out Insanity insanity))
        {
            insanity.ReduceSanity(sanityDecreasePercentage);
        }
        return true;
    }
}
