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

    public void DecreaseDurability()
    {
        uses -= 1;
        if (uses <= 0)
        {
            OnBreak();
        }
        //Destruction is handled in Inventory.cs
        //Component is destroyed next frame
    }

    protected virtual void OnBreak()
    {

    }
}
