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

    private void Update()
    {
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

    public void Init(SourceSettings settings, bool destroyobject = false)
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
            Destroy(source);
            Destroy(this);
        }
    }
}