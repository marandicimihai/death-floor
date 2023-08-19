using UnityEngine;

public class Item : SyncValues, IInteractable
{
    public bool IsInteractable { get => isInteractable; }
    [SyncValue] public ItemProperties properties;
    [SyncValue] [SaveValue] public int uses;
    [SyncValue] [SaveValue] [SerializeField] bool isInteractable = true;

    bool visible = true;
    Inventory playerInventory;

    protected virtual void Start()
    {
        playerInventory = FindObjectOfType<Inventory>();
    }

    private void OnValidate()
    {
        uses = properties.uses;
    }

    protected virtual void OnBreak()
    {
        try
        {
            ItemManager.RemoveFromPhysicalItems(this);
        }
        catch { Debug.Log("Item manager issue"); }
    }

    protected virtual void OnDestroy()
    {
        try
        {
            ItemManager.RemoveFromPhysicalItems(this);
        }
        catch { Debug.Log("Item manager issue"); }
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

    public bool OnInteractPerformed()
    {
        if (!IsInteractable) return false;

        if (playerInventory != null)
        {
            playerInventory.PickUpItem(this);
        }
        else
        {
            Debug.Log("No inventory");
        }
        
        return true;
    }

    public bool OnInteractCanceled()
    {
        if (!IsInteractable) return false;
        return true;
    }

    public string InteractionPrompt()
    {
        return !IsInteractable ? string.Empty : "Pick up";
    }
}
