using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
        }
        else
        {
            AudioManager.Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        Play("Background");
    }

    // Update is called once per frame
    public void Play(string name)
    {
        Sound findSound = Array.Find(sounds, sound => sound.name == name);
        if (findSound == null)
            return;
        findSound.source.Play();
    }

    public void Stop(string name)
    {
        Sound findSound = Array.Find(sounds, sound => sound.name == name);
        if (findSound == null)
            return;
        findSound.source.Stop();
    }
}
