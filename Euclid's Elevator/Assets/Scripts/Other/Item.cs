using UnityEngine;

public class Item : ItemBase
{
    public ItemObject itemObj;
    [Constant] public AudioSource[] dropSources;
    [System.NonSerialized] public int uses = -1;

    private void Awake()
    {
        if (uses == -1 && itemObj != null)
            uses = itemObj.uses;
    }

    public virtual void UseItem(FpsController controller)
    {

    }

    public void PlayDropSound()
    {
        int index = Random.Range(0, dropSources.Length);

        for (int i = 0; i < dropSources.Length; i++)
        {
            if (dropSources[index] == null)
            {
                index = (index + 1) % dropSources.Length;
                continue;
            }

            dropSources[index].Play();
            return;
        }
    }
}
