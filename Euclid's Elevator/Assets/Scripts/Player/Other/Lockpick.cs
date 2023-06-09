using UnityEngine;

public class Lockpick : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float lockPickTime;
    [SerializeField] float lockHoldTime;
    [SerializeField] float lockTime;

    Door target;

    float timeElapsed;

    bool picking;
    bool locking;

    private void Update()
    {
        if (!player.interactionManager.GetInteractionRaycast(out RaycastHit hit) ||
            hit.transform.GetComponentInParent<Door>() == null ||
            hit.transform.GetComponentInParent<Door>() != target)
        {
            StopSlider();
            picking = false;
            locking = false;
        }
        if (locking && !target.MatchesRequirement(player))
        {
            StopSlider();
            locking = false;
        }
        if (picking)
        {
            player.HUDManager.actionInfo.SetSliderValue(timeElapsed / lockPickTime, this);
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= lockPickTime)
            {
                StopSlider();
                target.OpenDoor(true);
                picking = false;
            }
        }
        if (locking)
        {
            if (timeElapsed >= lockHoldTime)
            {
                player.HUDManager.actionInfo.SetSliderValue((timeElapsed - lockHoldTime) / (lockTime - lockHoldTime), this);
            }

            timeElapsed += Time.deltaTime;
            if (timeElapsed >= lockTime)
            {
                StopSlider();
                target.LockDoor(player);
                locking = false;
            }
        }
    }

    public void PickLock(Door door)
    {
        if (!picking && !locking && !door.StageLocked)
        {
            player.HUDManager.actionInfo.StartAction(SliderType.Unlock, this);
            timeElapsed = 0;
            target = door;
            picking = true;
        }
    }

    public void Lock(Door door)
    {
        if (!picking && !locking && !door.Open)
        {
            player.HUDManager.actionInfo.StartAction(SliderType.Lock, this);
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

        StopSlider();

        picking = false;
        locking = false;
    }

    void StopSlider()
    {
        if (picking || locking)
        {
            player.HUDManager.actionInfo.StopAction(this);
        }
    }
}
