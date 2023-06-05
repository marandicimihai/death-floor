public interface IUsable
{
    public abstract bool OnUse();
}

public class Item : SyncValues
{
    [SyncValue] public ItemProperties properties;
    [SyncValue] public int uses;
    [SyncValue] bool itemInit;

    private void Start()
    {
        if (!itemInit)
        {
            uses = properties.uses;
            itemInit = true;
        }
    }
}
