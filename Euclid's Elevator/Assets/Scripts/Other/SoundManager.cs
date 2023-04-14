using System;
using System.Collections;
using UnityEngine;

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

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameManager.MakePausable(this);
    }

    public void PlaySound(string name, float playbackSpeed = 1)
    {
        Sound sound = Array.Find(sounds, (Sound current) => current.name == name);

        if (sound.name == null)
        {
            Debug.Log("Couldn't find sound");
            return;
        }

        sound.source.pitch = playbackSpeed;
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

    public void PlaySound(string[] randomSounds)
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

            sound.source.Play();
            return;
        }
    }

    public void PlaySounds(string[] soundsInOrder)
    {
        StartCoroutine(PlaySoundsInOrder(soundsInOrder));
    }

    IEnumerator PlaySoundsInOrder(string[] soundsInOrder)
    {
        for (int i = 0; i < soundsInOrder.Length; i++)
        {
            Sound sound = Array.Find(sounds, (Sound current) => current.name == soundsInOrder[i]);

            if (sound.name == null)
            {
                continue;
            }

            sound.source.Play();
            yield return new WaitForSeconds(sound.source.clip.length);
        }
    }
}
