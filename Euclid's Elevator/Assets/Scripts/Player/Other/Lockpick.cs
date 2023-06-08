using UnityEngine;

public class Lockpick : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float lockPickTime;
    [SerializeField] float lockHoldTime;
    [SerializeField] float lockTime;

    Door target;
    int keyIndex;

    float timeElapsed;

    bool picking;
    bool locking;

    private void Update()
    {
        if (player.interactionManager.GetInteractionRaycast().transform == null ||
            player.interactionManager.GetInteractionRaycast().transform.GetComponentInParent<Door>() == null ||
            player.interactionManager.GetInteractionRaycast().transform.GetComponentInParent<Door>() != target)
        {
            picking = false;
            locking = false;
        }
        if (player.inventory.Items[keyIndex] == null)
        {
            locking = false;
        }
        if (picking)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= lockPickTime)
            {
                target.OpenDoor(true);
                picking = false;
            }
        }
        if (locking)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= lockTime)
            {
                player.inventory.DecreaseDurability(keyIndex);
                target.LockDoor();
                locking = false;
            }
        }
    }

    public void PickLock(Door door)
    {
        if (!picking && !locking && !door.StageLocked)
        {
            timeElapsed = 0;
            target = door;
            picking = true;
        }
    }

    public void Lock(Door door, int keyIndex)
    {
        if (!picking && !locking && !door.Open)
        {
            this.keyIndex = keyIndex;
            timeElapsed = 0;
            target = door;
            locking = true;
        }
    }

    public void Stop()
    {
        if (locking && timeElapsed < lockHoldTime)
        {
            target.Toggle();
        }

        picking = false;
        locking = false;
    }
}
