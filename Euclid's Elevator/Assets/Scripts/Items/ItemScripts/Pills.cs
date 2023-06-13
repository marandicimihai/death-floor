using UnityEngine;

public class Pills : Item, IUsable
{
    [SerializeField] [SyncValue] float sanityDecreasePercentage;

    public bool OnUse(Player player)
    {
        player.insanity.ReduceSanity(sanityDecreasePercentage);
        return true;
    }
}
