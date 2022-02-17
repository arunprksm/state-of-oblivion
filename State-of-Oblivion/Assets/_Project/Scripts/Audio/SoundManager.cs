using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }


    [SerializeField] private AudioSource SFX;
    [SerializeField] private AudioSource MusicPlay;

    [SerializeField] private Slider musicVolume;
    [SerializeField] private Slider sfxVolume;

    [SerializeField] private SoundType[] Sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic(global::Sounds.Music);
        musicVolume.value = 0.5f;
        sfxVolume.value = 0.5f;

    }
    private void Update()
    {
        MusicPlay.volume = musicVolume.value;
        SFX.volume = sfxVolume.value;
    }

    public void PlayMusic(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            MusicPlay.clip = clip;
            MusicPlay.Play();
        }
        else
        {
            Debug.LogError("Clip not found on soundType: " + sound);
        }
    }
   
    public void PlaySFX(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            SFX.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found on soundType: " + sound);
        }
    }

    public void PlayerDeath(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            SFX.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found on soundType: " + sound);
        }
    }
    private AudioClip getSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundType == sound);

        if (item != null) return item.audioClip;

        return null;
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip audioClip;
}

public enum Sounds
{
    ButtonClick,
    LevelSelection,
    Music,
    PlayerMove,
    PlayerDeath,
    EnemyDeath,
    GameMusic_scene1,
}