using UnityEngine;

public class Lockpick : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] float lockPickTime;
    [SerializeField] float lockHoldTime;
    [SerializeField] float lockTime;

    [Header("Sounds")]
    [SerializeField] string picklock;

    Door target;

    float lockpickMultiplier;
    float timeElapsed;

    bool picking;
    bool locking;

    private void Awake()
    {
        lockpickMultiplier = 1;
    }

    private void Update()
    {
        if (!player.GetInteractionRaycast(out RaycastHit hit) ||
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
            player.SetSliderValue(timeElapsed / lockPickTime, this);
            timeElapsed += Time.deltaTime * lockpickMultiplier;
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
                player.SetSliderValue((timeElapsed - lockHoldTime) / (lockTime - lockHoldTime), this);
            }

            timeElapsed += Time.deltaTime * lockpickMultiplier;
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
            AudioManager.Instance.PlayClip(door.handle, picklock);
            player.StartAction(SliderType.Unlock, this);
            timeElapsed = 0;
            target = door;
            picking = true;
        }
    }

    public void Lock(Door door)
    {
        if (!picking && !locking && !door.Open)
        {
            player.StartAction(SliderType.Lock, this);
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
            player.StopAction(this);
        }
        AudioManager.Instance.StopClipsWithName(picklock);
    }

    public void BoostForTime(float multiplier, float time)
    {
        if (lockpickMultiplier != 1)
        {
            CancelInvoke(nameof(WearOffBoost));
        }
        lockpickMultiplier = multiplier;
        Invoke(nameof(WearOffBoost), time);
    }

    void WearOffBoost()
    {
        lockpickMultiplier = 1;
    }
}
