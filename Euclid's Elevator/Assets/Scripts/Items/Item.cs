public class Item : SyncValues
{
    [SyncValue] public ItemProperties properties;
    [System.NonSerialized] [SyncValue] public int uses;
    [SyncValue] bool itemInit;

    private void Start()
    {
        if (!itemInit)
        {
            uses = properties.uses;
            itemInit = true;
        }
    }

    public virtual bool OnUse()
    {
        return false;
    }
}
