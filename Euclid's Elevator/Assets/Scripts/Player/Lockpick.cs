using System.Collections;
using UnityEngine;
using System;

public class Lockpick : MonoBehaviour
{
    [SerializeField] ActionBar bar;
    [SerializeField] float pickDuration;
    [SerializeField] float lockDuration;
    [SerializeField] float doorLockHoldTime;

    float boost;
    bool picking;
    bool locking;


    private void Awake()
    {
        boost = 1;
    }

    public void PickLock(Door door, InteractionSettings settings)
    {
        if (picking || !door.locked)
            return;

        StartCoroutine(PickLockC(door, settings));
    }

    IEnumerator PickLockC(Door door, InteractionSettings settings)
    {
        bar.StartAction("PickLock");
        picking = true;
        SoundManager.Instance.PlaySound("LockPick", boost);
        float timeElapsed = 0;

        while (timeElapsed < pickDuration)
        {
            timeElapsed += Time.deltaTime * boost;

            if (!Physics.Raycast(settings.cam.transform.position, settings.cam.transform.forward, out RaycastHit hit, settings.interactionDistance, settings.interactionMask) ||
                !hit.transform.CompareTag("Door"))
            {
                StopPicking();
                yield break;
            }
            else if (!door.locked || !picking)
            {
                StopPicking();
                yield break;
            }
            
            bar.SetSliderValue("PickLock", (timeElapsed) / (pickDuration));

            yield return null;
        }

        door.ForceOpen();
        StopPicking();
    }

    public void StopPicking()
    {
        bar.StopAction("PickLock");
        SoundManager.Instance.StopSound("LockPick");
        picking = false;
    }
    
    public void LockLock(Door door, InteractionSettings settings, Inventory inventory, ItemObject itemObj)
    {
        if (door.locked || door.Open || locking)
            return;

        StartCoroutine(LockLockC(door, settings, inventory, itemObj));
    }

    IEnumerator LockLockC(Door door, InteractionSettings settings, Inventory inventory, ItemObject itemObj)
    {
        locking = true;
        float timeElapsed = 0;

        while (timeElapsed < lockDuration)
        {
            timeElapsed += Time.deltaTime * boost;

            if (!Physics.Raycast(settings.cam.transform.position, settings.cam.transform.forward, out RaycastHit hit, settings.interactionDistance, settings.interactionMask) ||
                !hit.transform.CompareTag("Door"))
            {
                StopLocking();
                yield break;
            }
            else if (door.locked || door.Open || inventory.Items[inventory.ActiveSlot] == null || inventory.Items[inventory.ActiveSlot].itemObj.name != itemObj.name)
            {
                StopLocking();
                yield break;
            }
            else if (!locking)
            {
                if (timeElapsed < doorLockHoldTime)
                {
                    door.Toggle();
                }
                yield break;
            }
            if (timeElapsed >= doorLockHoldTime)
            {
                bar.StartAction("LockLock");
                bar.SetSliderValue("LockLock", (timeElapsed - doorLockHoldTime) / (lockDuration - doorLockHoldTime));
            }

            yield return null;
        }

        inventory.UseItem(inventory.ActiveSlot);
        door.ForceLock();
        StopLocking();
    }

    public void StopLocking()
    {
        bar.StopAction("LockLock");
        locking = false;
    }

    public void BoostLockPick(float multiplier, float time)
    {
        boost = multiplier;
        StartCoroutine(WaitAndExec(time, () =>
        {
            boost = 1;
        }));
    }

    IEnumerator WaitAndExec(float time, Action exec, bool repeat = false)
    {
        yield return new WaitForSeconds(time);
        exec?.Invoke();

        if (repeat)
        {
            StartCoroutine(WaitAndExec(time, exec, repeat));
        }
    }
}