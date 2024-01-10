using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct SourceSettings
{
    public string name;
    public AudioClip clip;
    public AudioMixerGroup group;
    public bool playOnAwake;
    public bool loop;
    [Range(0, 1)] public float volume;
    [Range(0, 1)] public float pitch;
    [Range(0, 1)] public float spatialBlend;
    public float maxDistance;
    public bool playPaused;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<AudioJob> jobs;

    [SerializeField] LayerMask solid;
    [SerializeField] AudioMixerSnapshot paused;//muffles the sound
    [SerializeField] AudioMixerSnapshot unpaused;
    [SerializeField] AudioMixer main;
    [SerializeField] SourceSettings[] settings;
    [SerializeField] GameObject empty;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        jobs = new();
        Instance = this;
    }

    private void Start()
    {
        TryPauseEvents();
        SceneManager.activeSceneChanged += (Scene first, Scene second) =>
        {
            TryPauseEvents();
        };
    }

    void SetVolume(float effectsVolume, float ambienceVolume)
    {
        effectsVolume = -40 + effectsVolume * 40;
        ambienceVolume = -40 + ambienceVolume * 40;


        main.SetFloat("effectsVolume", effectsVolume);
        main.SetFloat("ambienceVolume", ambienceVolume);
    }

    void TryPauseEvents()
    {
        try
        {
            PauseGame.Instance.OnPause += (object caller, EventArgs args) =>
            {
                Pause();
            };
            PauseGame.Instance.OnUnPause += (object caller, EventArgs args) =>
            {
                Unpause();
            };
        }
        catch
        {
            
        }
    }

    void Pause()
    {
        paused.TransitionTo(0);
        foreach (AudioJob job in jobs)
        {
            if (!job.playPaused && job.source != null)
            {
                job.source.Pause();
            }
        }
    }

    void Unpause()
    {
        unpaused.TransitionTo(0);
        foreach (AudioJob job in jobs)
        {
            if (job.source != null)
            {
                job.source.UnPause();
            }
        }
    }

    public AudioJob PlayClip(string name)
    {
        if (name == string.Empty)
        {
            return null;
        }

        AudioJob job = Instantiate(empty).AddComponent<AudioJob>();
        job.Init(GetSettings(name), solid, true);
        return job;
    }

    public AudioJob PlayClip(Vector3 position, string name, bool audioocclusion = true)
    {
        if (name == string.Empty)
        {
            return null;
        }

        AudioJob job = Instantiate(empty, position, Quaternion.identity).AddComponent<AudioJob>();
        job.Init(GetSettings(name), solid, true, audioocclusion: audioocclusion);
        return job;
    }

    public AudioJob PlayClip(GameObject parent, string name, bool audioocclusion = true)
    {
        if (name == string.Empty)
        {
            return null;
        }

        AudioJob job = parent.AddComponent<AudioJob>();
        job.Init(GetSettings(name), solid, audioocclusion: audioocclusion);
        return job;
    }

    public AudioJob PlayRandomClip(string[] names)
    {
        if (names.Length == 0)
        {
            return null;
        }

        int i = UnityEngine.Random.Range(0, names.Length);
        AudioJob job = Instantiate(empty).AddComponent<AudioJob>();
        job.Init(GetSettings(names[i]), solid, true);
        return job;

    }

    public AudioJob PlayRandomClip(Vector3 position, string[] names, bool audioocclusion = true)
    {
        if (names.Length == 0)
        {
            return null;
        }

        int i = UnityEngine.Random.Range(0, names.Length);
        AudioJob job = Instantiate(empty, position, Quaternion.identity).AddComponent<AudioJob>();
        job.Init(GetSettings(names[i]), solid, true, audioocclusion: audioocclusion);
        return job;
    }

    public AudioJob PlayRandomClip(GameObject parent, string[] names, bool audioocclusion = true)
    {
        if (names.Length == 0)
        {
            return null;
        }

        int i = UnityEngine.Random.Range(0, names.Length);
        AudioJob job = parent.AddComponent<AudioJob>();
        job.Init(GetSettings(names[i]), solid, audioocclusion: audioocclusion);
        return job;
    }

    public void PlayClips(string[] names)
    {
        if (names.Length == 0)
        {
            return;
        }

        float time = 0;
        for (int i = 0; i < names.Length; i++)
        {
            StartCoroutine(PlayClipInSeconds(GetSettings(names[i]), time));
            time += GetSettings(names[i]).clip.length;
        }
    }

    public void PlayClips(Vector3 position, string[] names, bool audioocclusion = true)
    {
        if (names.Length == 0)
        {
            return;
        }

        float time = 0;
        for (int i = 0; i < names.Length; i++)
        {
            StartCoroutine(PlayClipInSeconds(position, GetSettings(names[i]), time, audioocclusion: audioocclusion));
            time += GetSettings(names[i]).clip.length;
        }
    }

    public void PlayClips(GameObject parent, string[] names, bool audioocclusion = true)
    {
        if (names.Length == 0)
        {
            return;
        }

        float time = 0;
        for (int i = 0; i < names.Length; i++)
        {
            StartCoroutine(PlayClipInSeconds(parent, GetSettings(names[i]), time, audioocclusion: audioocclusion));
            time += GetSettings(names[i]).clip.length;
        }
    }

    public void StopClipsWithName(string name)
    {
        foreach (AudioJob job in jobs.ToArray())
        {
            if (job.name == name)
            {
                job.StopPlaying();
            }
        }
    }

    public void FadeAwayClip(AudioJob job, float time, Action action = null)
    {
        if (job != null)
        {
            job.FadeAway(time, action);
        }
    }

    public void StopClip(AudioJob job)
    {
        if (job != null)
        {
            job.StopPlaying();
        }
    }

    SourceSettings GetSettings(string name)
    {
        return Array.Find(settings, (SourceSettings settings) => settings.name == name);
    }

    IEnumerator PlayClipInSeconds(SourceSettings settings, float time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(empty).AddComponent<AudioJob>().Init(settings, solid, true);
    }

    IEnumerator PlayClipInSeconds(Vector3 position, SourceSettings settings, float time, bool audioocclusion = true)
    {
        yield return new WaitForSeconds(time);

        Instantiate(empty, position, Quaternion.identity).AddComponent<AudioJob>().Init(settings, solid, true, audioocclusion: audioocclusion);
    }

    IEnumerator PlayClipInSeconds(GameObject parent, SourceSettings settings, float time, bool audioocclusion = true)
    {
        yield return new WaitForSeconds(time);

        parent.AddComponent<AudioJob>().Init(settings, solid, audioocclusion: audioocclusion);
    }
}
