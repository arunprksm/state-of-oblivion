using System;
using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }
    public GameObject optionMenu;

    [SerializeField] internal AudioSource MusicPlay;
    [SerializeField] internal AudioSource SFX;

    [SerializeField] internal Slider musicVolume;
    [SerializeField] internal Slider sfxVolume;
    

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
        CurrentVolume();
    }
    private void CurrentVolume()
    {
        musicVolume.value = GameManager.Instance.currentMusicVolume;
        sfxVolume.value = GameManager.Instance.currentSfxVolume;
        //musicSlider.GetComponent<Slider>().value = GameManager.Instance.musicVolume;
        //sfxSlider.GetComponent<Slider>().value = GameManager.Instance.sfxVolume;
    }
    private void SetVolume()
    {
        GameManager.Instance.currentMusicVolume = musicVolume.value;
        GameManager.Instance.currentSfxVolume = sfxVolume.value;
    }
    private void VolumeControl()
    {
        MusicPlay.volume = musicVolume.value;
        SFX.volume = sfxVolume.value;
    }

    private void Update()
    {
        VolumeControl();
        SetVolume();
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
    public void PauseMusic(Sounds sound)
    {
        AudioClip clip = getSoundClip(sound);
        if (clip != null)
        {
            MusicPlay.clip = clip;
            MusicPlay.Pause();
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
    None,
    ButtonClick,
    LevelSelection,
    Music,
    PlayerMove,
    PlayerAttack,
    playerJump,
    PlayerDeath,
    EnemyDeath,
    GameMusic_scene1,
    GameMusic_scene2,
    GameMusic_scene3,
}