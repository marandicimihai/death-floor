using UnityEngine;

public class Pills : Item, IUsable
{
    [SerializeField] [SyncValue] float sanityDecreasePercentage;

    Insanity insanity;

    private void Start()
    {
        insanity = FindObjectOfType<Insanity>();
    }

    public bool OnUse()
    {
        if (insanity != null)
        {
            insanity.ReduceSanity(sanityDecreasePercentage);
        }
        else
        {
            Debug.Log("No insanity");
        }
        return true;
    }
}
