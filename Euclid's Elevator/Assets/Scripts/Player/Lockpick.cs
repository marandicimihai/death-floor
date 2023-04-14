using System.Collections;
using UnityEngine;
using System;

public class Lockpick : MonoBehaviour
{
    [SerializeField] ActionBar bar;
    [SerializeField] float pickDuration;

    float pickTimeBoost;
    float pickPercent;

    float timeElapsed;
    float time;
    bool picking;

    float dist;
    LayerMask mask;
    Camera cam;
    Door door;

    private void Awake()
    {
        pickTimeBoost = 1;
    }

    private void Update()
    {
        if (picking)
        {
            timeElapsed += Time.deltaTime * pickTimeBoost;
            bar.SetSliderValue(pickPercent);
            if (time < time - pickDuration + timeElapsed)
            {
                StopPicking();
                door.ForceOpen();
                pickPercent = 100;
            }
            else if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, dist, mask) ||
                     !hit.transform.CompareTag("Door"))
            {
                StopPicking();
            }
            else if (!door.locked)
            {
                StopPicking();
            }
            else
            {
                pickPercent = (timeElapsed) / (pickDuration) * 100;
            }
        }
        else
        {
            timeElapsed = 0;
            pickPercent = 0;
        }
    }

    public void PickLock(Door door, InteractionSettings settings)
    {
        if (picking)
            return;

        this.door = door;
        if (!door.locked)
            return;

        bar.StartAction();
        time = Time.time + pickDuration;

        cam = settings.cam;
        dist = settings.interactionDistance;
        mask = settings.interactionMask;

        SoundManager.Instance.PlaySound("LockPick", pickTimeBoost);
        picking = true;
    }

    public void StopPicking()
    {
        bar.StopAction();
        SoundManager.Instance.StopSound("LockPick");
        picking = false;
    }
    
    public void BoostLockPick(float multiplier, float time)
    {
        pickTimeBoost = multiplier;
        StartCoroutine(WaitAndExec(time, () =>
        {
            pickTimeBoost = 1;
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