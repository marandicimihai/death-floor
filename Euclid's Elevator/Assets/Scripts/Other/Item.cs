public class Item : ItemBase
{
    public ItemObject itemObj;
    public int uses = -1;

    private void Awake()
    {
        if (uses == -1 && itemObj != null)
            uses = itemObj.uses;
    }

    public virtual void UseItem(FpsController controller)
    {

    }
}
