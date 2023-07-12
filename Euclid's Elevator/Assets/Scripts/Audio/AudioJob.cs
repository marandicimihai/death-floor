using UnityEngine;

public class AudioJob : MonoBehaviour
{
    public new string name;
    public AudioSource source;
    public bool playPaused;
    bool ddol;
    bool destroyobject;

    System.Action action;
    bool stopqueued;
    float time;

    bool fadequeued;
    float fadeTime;

    float initialSourceVolume;

    LayerMask solid;
    AudioLowPassFilter filter;
    float interpolation;
    bool occlude;
    bool isoccluding;
    bool occlusionInitialized;

    private void Update()
    {
        if (occlude)
        {
            if (source != null)
            {
                if (AudioOcclusionTarget.target != null && !isoccluding)
                {
                    filter = gameObject.AddComponent<AudioLowPassFilter>();
                    isoccluding = true;
                }
                else if (AudioOcclusionTarget.target != null)
                {
                    if (filter != null && AudioOcclusionTarget.target != null)
                    {
                        Vector3 listener = AudioOcclusionTarget.target.position;
                        if (!occlusionInitialized)
                        {
                            if (!Physics.Raycast(transform.position, listener - transform.position, Vector3.Distance(transform.position, listener), solid))
                            {
                                filter.cutoffFrequency = 22000;
                                interpolation = 1;
                            }
                            else
                            {
                                filter.cutoffFrequency = 5000;
                                interpolation = 0;
                            }
                            occlusionInitialized = true;
                        }

                        if (!Physics.Raycast(transform.position, listener - transform.position, Vector3.Distance(transform.position, listener), solid))
                        {
                            interpolation += Time.deltaTime;
                        }
                        else
                        {
                            interpolation -= Time.deltaTime;
                        }
                        interpolation = Mathf.Clamp01(interpolation);
                        filter.cutoffFrequency = Mathf.Lerp(5000, 22000, interpolation);
                    }
                }
            }
        }
        if (stopqueued)
        {
            time += Time.deltaTime;
            if (time >= source.clip.length)
            {
                action?.Invoke();
                StopPlaying();
            }
        }
        if (fadequeued)
        {
            time += Time.deltaTime;
            source.volume = (1 - (time / fadeTime)) * initialSourceVolume;
            if (time >= fadeTime)
            {
                action?.Invoke();
                StopPlaying();
            }
        }
    }

    public void Init(SourceSettings settings, LayerMask solid, bool destroyobject = false, bool audioocclusion = false)
    {
        AudioManager.Instance.jobs.Add(this);
        this.name = settings.name;
        source = gameObject.AddComponent<AudioSource>();
        source.clip = settings.clip;
        source.outputAudioMixerGroup = settings.group;
        source.playOnAwake = settings.playOnAwake;
        source.loop = settings.loop;
        source.volume = settings.volume;
        source.pitch = settings.pitch;
        source.spatialBlend = settings.spatialBlend;
        source.maxDistance = settings.maxDistance;
        source.rolloffMode = AudioRolloffMode.Linear;

        this.playPaused = settings.playPaused;
        this.destroyobject = destroyobject;

        source.Play();
        if (!source.loop)
        {
            Invoke(nameof(StopPlaying), source.clip.length + 0.2f);
        }

        this.solid = solid;
        if (audioocclusion && TryGetComponent<AudioLowPassFilter>(out AudioLowPassFilter lowpass))
        {
            filter = lowpass;
            occlusionInitialized = true;
            isoccluding = true;
            occlude = true;
        }
        else if (audioocclusion)
        {
            occlude = true;
        }
    }

    public void DDOL()
    {
        gameObject.AddComponent<DDOL>();
    }

    public void StopOnClipEnd(System.Action action)
    {
        if (stopqueued || fadequeued)
        {
            return;
        }

        stopqueued = true;
        this.action = action;
        time = source.time;
    }

    public void FadeAway(float time, System.Action action = null)
    {
        if (stopqueued || fadequeued)
        {
            return;
        }

        initialSourceVolume = source.volume;
        fadequeued = true;
        this.action = action;
        fadeTime = time;
        this.time = 0;
    }

    public void StopPlaying()
    {
        AudioManager.Instance.jobs.Remove(this);
        CancelInvoke();
        source.Stop();
        if (destroyobject)
        {
            Destroy(gameObject);
        }
        else
        {
            Destroy(filter);
            Destroy(source);
            Destroy(this);
        }
    }
}