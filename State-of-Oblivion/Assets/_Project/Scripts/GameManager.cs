using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private SceneController sceneController;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    [SerializeField] internal float musicVolume;
    [SerializeField] internal float sfxVolume;
    [SerializeField] internal float currentMusicVolume;
    [SerializeField] internal float currentSfxVolume;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            musicVolume = 0.5f;
            sfxVolume = 0.8f;
            currentMusicVolume = musicVolume;
            currentSfxVolume = sfxVolume;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
