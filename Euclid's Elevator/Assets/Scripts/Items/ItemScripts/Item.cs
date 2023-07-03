using UnityEngine;

public class Item : SyncValues
{
    [SyncValue] public ItemProperties properties;
    [SyncValue] public int uses;

    bool visible = true;

    private void Start()
    {
        SetVisible(visible);
    }

    private void OnValidate()
    {
        uses = properties.uses;
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

    public void SetVisible(bool visible)
    {
        if (visible && !this.visible)
        {
            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = true;
            }
            this.visible = true;
        }
        else if (!visible && this.visible)
        {
            foreach (Renderer rend in GetComponentsInChildren<Renderer>())
            {
                rend.enabled = false;
            }
            this.visible = false;
        }
    }

    protected virtual void OnBreak()
    {

    }
}
