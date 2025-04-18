using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<SoundGroup> soundGroups;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        foreach (var group in soundGroups)
        {
            foreach (var sound in group.sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        Play("Background");
    }

    public void Stop(string name)
    {
        Sound sound = FindSoundByName(name);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

public void Play(string groupName)
{
    var group = soundGroups.Find(g => g.groupName == groupName);
    if (group == null || group.sounds.Count == 0) return;

    int randomIndex = UnityEngine.Random.Range(0, group.sounds.Count);
    var randomSound = group.sounds[randomIndex];

    randomSound.source.Play();
}

    public void StopGroup(string groupName)
    {
        var group = soundGroups.Find(g => g.groupName == groupName);
        if (group == null) return;

        foreach (var sound in group.sounds)
        {
            sound.source.Stop();
        }
    }

    private Sound FindSoundByName(string name)
    {
        foreach (var group in soundGroups)
        {
            var sound = group.sounds.Find(s => s.name == name);
            if (sound != null) return sound;
        }
        return null;
    }
}
