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

    [SyncValue] [SaveValue] bool winding;
    [SyncValue] [SaveValue] bool ringing;
    [SyncValue] [SaveValue] bool ticking;
    [SyncValue] [SaveValue] bool started;
    [SyncValue] [SaveValue] bool destroy;

    [SyncValue] [SaveValue] float windTimeElapsed;
    [SyncValue] [SaveValue] float tickTimeElapsed;
    [SyncValue] [SaveValue] float ringTimeElapsed;

    EnemyNavigation navigation;

    private void Start()
    {
        navigation = FindObjectOfType<EnemyNavigation>();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnElevatorDoorClosed += DestroyTimer;
            GameManager.Instance.OnDeath += DestroyTimer;
        }
        else
        {
            Debug.Log("No game manager");
        }
    }

    public void DestroyTimer(object caller, System.EventArgs args)
    { 
        if (started) 
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnElevatorDoorClosed -= DestroyTimer;
                GameManager.Instance.OnDeath -= DestroyTimer;
            }
            else
            {
                Debug.Log("No game manager");
            }
            if (windJob != null)
            {
                AudioManager.Instance.StopClip(windJob);
            }
            if (tickJob != null)
            {
                AudioManager.Instance.StopClip(tickJob);
            }
            if (ringJob != null)
            {
                AudioManager.Instance.StopClip(ringJob);
            }

            destroy = true;
        }
    }

    private void Update()
    {
        if (destroy)
        {
            Destroy(gameObject);
            return;
        }
        if (winding)
        {
            if (windJob != null)
            {
                windJob.transform.position = transform.position;
            }
            else
            {
                windJob = AudioManager.Instance.PlayClip(transform.position, wind);
            }
            windTimeElapsed += Time.deltaTime;
            knob.localRotation = Quaternion.Lerp(Quaternion.Euler(unwindedKnobAngles), Quaternion.Euler(windedKnobAngles), windTimeElapsed / windDuration);
            if (windTimeElapsed >= windDuration)
            {
                winding = false;
                ticking = true;
                AudioManager.Instance.StopClip(windJob);
                tickJob = AudioManager.Instance.PlayClip(transform.position, tick);
            }
        }
        if (ticking)
        {
            if (tickJob != null)
            {
                tickJob.transform.position = transform.position;
            }
            else
            {
                tickJob = AudioManager.Instance.PlayClip(transform.position, tick);
            }
            knob.localRotation = Quaternion.Lerp(Quaternion.Euler(windedKnobAngles), Quaternion.Euler(unwindedKnobAngles), tickTimeElapsed / duration);
            tickTimeElapsed += Time.deltaTime;
            if (tickTimeElapsed >= duration)
            {
                ticking = false;
                ringing = true;
                AudioManager.Instance.StopClip(tickJob);
                ringJob = AudioManager.Instance.PlayClip(transform.position, ring);
            }
        }
        if (ringing)
        {
            if (ringJob != null)
            {
                ringJob.transform.position = transform.position;
            }
            else
            {
                ringJob = AudioManager.Instance.PlayClip(transform.position, ring);
            }
            ringTimeElapsed += Time.deltaTime;

            if (navigation != null)
            {
                navigation.InspectNoise(transform.position, true);
            }
            else
            {
                Debug.Log("No enemy navigation.");
            }

            if (ringTimeElapsed >= ringDuration)
            {
                AudioManager.Instance.StopClip(ringJob);
                ringing = false;
                gameObject.AddComponent<Lifetime>().Initiate(4);
                try
                {
                    ItemManager.RemoveFromPhysicalItems(this);
                }
                catch { Debug.Log("Item manager issue"); }
            }
        }
    }

    public bool OnUse()
    {
        if (!started)
        {
            windJob = AudioManager.Instance.PlayClip(transform.position, wind);
            winding = true;
            started = true;
        }
        return false;
    }
}
