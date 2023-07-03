using UnityEngine;

public class Timer : Item, IUsable
{
    [SerializeField] Transform knob;
    [SerializeField] [SyncValue] float duration;
    [SerializeField] [SyncValue] float windDuration;
    [SerializeField] [SyncValue] float ringDuration;
    [SerializeField] [SyncValue] Vector3 unwindedKnobAngles;
    [SerializeField] [SyncValue] Vector3 windedKnobAngles;

    [Header("Audio")]
    [SerializeField] [SyncValue] string wind;
    [SerializeField] [SyncValue] string tick;
    [SerializeField] [SyncValue] string ring;

    [SyncValue] AudioJob windJob;
    [SyncValue] AudioJob tickJob;
    [SyncValue] AudioJob ringJob;

    [SyncValue] bool winding;
    [SyncValue] bool ringing;
    [SyncValue] bool ticking;
    [SyncValue] bool started;

    [SyncValue] float windTimeElapsed;
    [SyncValue] float tickTimeElapsed;
    [SyncValue] float ringTimeElapsed;

    private void Update()
    {
        if (winding)
        {
            if (windJob != null)
            {
                windJob.transform.position = transform.position;
            }
            windTimeElapsed += Time.deltaTime;
            knob.localRotation = Quaternion.Lerp(Quaternion.Euler(unwindedKnobAngles), Quaternion.Euler(windedKnobAngles), windTimeElapsed / windDuration);
            if (windTimeElapsed >= windDuration)
            {
                winding = false;
                ticking = true;
                AudioManager.Instance.StopClip(windJob);
                tickJob = AudioManager.Instance.PlayClip(tick);
            }
        }
        if (ticking)
        {
            if (tickJob != null)
            {
                tickJob.transform.position = transform.position;
            }
            knob.localRotation = Quaternion.Lerp(Quaternion.Euler(windedKnobAngles), Quaternion.Euler(unwindedKnobAngles), tickTimeElapsed / duration);
            tickTimeElapsed += Time.deltaTime;
            if (tickTimeElapsed >= duration)
            {
                ticking = false;
                ringing = true;
                AudioManager.Instance.StopClip(tickJob);
                ringJob = AudioManager.Instance.PlayClip(ring);
            }
        }
        if (ringing)
        {
            if (ringJob != null)
            {
                ringJob.transform.position = transform.position;
            }
            ringTimeElapsed += Time.deltaTime;
            GameManager.Instance.enemy.InspectNoise(transform.position, true, true);
            if (ringTimeElapsed >= ringDuration)
            {
                AudioManager.Instance.StopClip(ringJob);
                ringing = false;
                gameObject.AddComponent<Lifetime>().Initiate(4);
            }
        }
    }

    public bool OnUse(Player player)
    {
        if (!started)
        {
            windJob = AudioManager.Instance.PlayClip(wind);
            winding = true;
            started = true;
        }
        return false;
    }
}
