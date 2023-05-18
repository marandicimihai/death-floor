using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[Serializable]
struct Sound
{
    public string name;
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] Sound[] sounds;
    public List<AudioSource> realtime;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.MakePausable(this);
    }

    public AudioSource GetSound(string name)
    {
        Sound sound = Array.Find(sounds, (Sound current) => current.name == name);

        return sound.source;
    }

    public void PlaySound(string name, float playbackSpeed = 1, bool realtime = false)
    {
        Sound sound = Array.Find(sounds, (Sound current) => current.name == name);

        if (sound.name == null)
        {
            Debug.Log("Couldn't find sound");
            return;
        }

        sound.source.pitch = playbackSpeed;
        if (realtime)
        {
            StartCoroutine(PlayRT(sound.source));
            return;
        }
        sound.source.Play();
    }
    
    public void PlaySound(string name, bool realtime = false)
    {
        Sound sound = Array.Find(sounds, (Sound current) => current.name == name);

        if (sound.name == null)
        {
            Debug.Log("Couldn't find sound");
            return;
        }

        if (realtime)
        {
            StartCoroutine(PlayRT(sound.source));
            return;
        }
        sound.source.Play();
    }
    
    public void PlaySound(string name)
    {
        Sound sound = Array.Find(sounds, (Sound current) => current.name == name);

        if (sound.name == null)
        {
            Debug.Log("Couldn't find sound");
            return;
        }

        sound.source.Play();
    }

    public void StopSound(string name)
    {
        Sound sound = Array.Find(sounds, (Sound current) => current.name == name);

        if (sound.name == null)
        {
            Debug.Log("Couldn't find sound");
            return;
        }

        sound.source.Stop();
    }

    public void StopSoundLoop(string name, Action onStopLoop = null)
    {
        Sound sound = Array.Find(sounds, (Sound current) => current.name == name);

        if (sound.name == null)
        {
            Debug.Log("Couldn't find sound");
            return;
        }

        StartCoroutine(StopLoop(sound, onStopLoop));
    }

    public void PlaySound(string[] randomSounds, bool realtime = false)
    {
        int index = UnityEngine.Random.Range(0, randomSounds.Length);

        for (int i = 0; i < randomSounds.Length; i++)
        {
            Sound sound = Array.Find(sounds, (Sound current) => current.name == randomSounds[index]);

            if (sound.name == null)
            {
                index = (index + 1) % randomSounds.Length;
                continue;
            }

            if (realtime)
            {
                StartCoroutine(PlayRT(sound.source));
                return;
            }
            sound.source.Play();
            return;
        }
    }

    public void PlaySounds(string[] soundsInOrder, bool realtime = false)
    {
        StartCoroutine(PlaySoundsInOrder(soundsInOrder, realtime));
    }

    IEnumerator PlaySoundsInOrder(string[] soundsInOrder, bool realtime = false)
    {
        for (int i = 0; i < soundsInOrder.Length; i++)
        {
            Sound sound = Array.Find(sounds, (Sound current) => current.name == soundsInOrder[i]);

            if (sound.name == null)
            {
                continue;
            }

            if (realtime)
            {
                StartCoroutine(PlayRT(sound.source));
            }
            else
            {
                sound.source.Play();
            }
            yield return new WaitForSeconds(sound.source.clip.length);
        }
    }

    IEnumerator PlayRT(AudioSource s)
    {
        realtime.Add(s);
        s.UnPause();
        s.Play();
        yield return new WaitForSeconds(s.clip.length);
        realtime.Remove(s);
    }

    IEnumerator StopLoop(Sound sound, Action onStopLoop)
    {
        float t = sound.source.time;
        if (t > 0)
        {
            while (t < sound.source.clip.length)
            {
                t += Time.deltaTime * sound.source.pitch;
                yield return null;
            }
        }
        sound.source.Stop();
        onStopLoop?.Invoke();
    }
}
