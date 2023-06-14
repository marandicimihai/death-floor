using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Audio;
using UnityEngine;

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

    [SerializeField] AudioMixerSnapshot paused;
    [SerializeField] AudioMixerSnapshot unpaused;
    [SerializeField] SourceSettings[] settings;
    [SerializeField] GameObject empty;

    private void Awake()
    {
        jobs = new();
        Instance = this;
    }

    public void Pause()
    {
        paused.TransitionTo(0);
        foreach (AudioJob job in jobs)
        {
            if (!job.playPaused)
            {
                job.source.Pause();
            }
        }
    }

    public void Unpause()
    {
        unpaused.TransitionTo(0);
        foreach (AudioJob job in jobs)
        {
            job.source.UnPause();
        }
    }

    public AudioJob PlayClip(string name)
    {
        if (name == string.Empty)
        {
            return null;
        }

        AudioJob job = Instantiate(empty).AddComponent<AudioJob>();
        job.Init(GetSettings(name), true);
        return job;
    }

    public AudioJob PlayClip(Vector3 position, string name)
    {
        if (name == string.Empty)
        {
            return null;
        }

        AudioJob job = Instantiate(empty, position, Quaternion.identity).AddComponent<AudioJob>();
        job.Init(GetSettings(name), true);
        return job;
    }

    public AudioJob PlayClip(GameObject parent, string name)
    {
        if (name == string.Empty)
        {
            return null;
        }

        AudioJob job = parent.AddComponent<AudioJob>();
        job.Init(GetSettings(name));
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
        job.Init(GetSettings(names[i]), true);
        return job;

    }

    public AudioJob PlayRandomClip(Vector3 position, string[] names)
    {
        if (names.Length == 0)
        {
            return null;
        }

        int i = UnityEngine.Random.Range(0, names.Length);
        AudioJob job = Instantiate(empty, position, Quaternion.identity).AddComponent<AudioJob>();
        job.Init(GetSettings(names[i]), true);
        return job;
    }

    public AudioJob PlayRandomClip(GameObject parent, string[] names)
    {
        if (names.Length == 0)
        {
            return null;
        }

        int i = UnityEngine.Random.Range(0, names.Length);
        AudioJob job = parent.AddComponent<AudioJob>();
        job.Init(GetSettings(names[i]));
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

    public void PlayClips(Vector3 position, string[] names)
    {
        if (names.Length == 0)
        {
            return;
        }

        float time = 0;
        for (int i = 0; i < names.Length; i++)
        {
            StartCoroutine(PlayClipInSeconds(position, GetSettings(names[i]), time));
            time += GetSettings(names[i]).clip.length;
        }
    }

    public void PlayClips(GameObject parent, string[] names)
    {
        if (names.Length == 0)
        {
            return;
        }

        float time = 0;
        for (int i = 0; i < names.Length; i++)
        {
            StartCoroutine(PlayClipInSeconds(parent, GetSettings(names[i]), time));
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

        Instantiate(empty).AddComponent<AudioJob>().Init(settings, true);
    }

    IEnumerator PlayClipInSeconds(Vector3 position, SourceSettings settings, float time)
    {
        yield return new WaitForSeconds(time);

        Instantiate(empty, position, Quaternion.identity).AddComponent<AudioJob>().Init(settings, true);
    }

    IEnumerator PlayClipInSeconds(GameObject parent, SourceSettings settings, float time)
    {
        yield return new WaitForSeconds(time);

        parent.AddComponent<AudioJob>().Init(settings);
    }
}
