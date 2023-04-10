using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
struct Sound
{
    public string name;
    public AudioSource source;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        instance = this;
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
}
