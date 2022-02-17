using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static bool IsGamePaused = false;

    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject optionMenu;


    [SerializeField] internal Slider SceneMusicVolume;
    [SerializeField] internal Slider SceneSfxVolume;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        optionMenu.SetActive(false);
        SetVolume();

        //SceneMusicVolume.value = SoundManager.Instance.musicVolume.value;
        //Debug.Log(SoundManager.Instance.musicVolume.value);
        //SceneSfxVolume.value = SoundManager.Instance.sfxVolume.value;
    }
    private void SetVolume()
    {
        SceneMusicVolume.value = SoundManager.Instance.musicVolume.value;
        //Debug.Log(SoundManager.Instance.musicVolume.value);
        SceneSfxVolume.value = SoundManager.Instance.sfxVolume.value;

    }
    private void VolumeControl()
    {
        SoundManager.Instance.MusicPlay.volume = SceneMusicVolume.value;
        SoundManager.Instance.SFX.volume = SceneSfxVolume.value;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        VolumeControl();
    }
    
    public void Resume()
    {
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    private void Pause()
    {
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void OptionButton()
    {
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        if (optionMenu.activeSelf)
        {
            optionMenu.SetActive(false);
        }
        else
        {
            optionMenu.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovementControl>() != null)
        {
            StartCoroutine(NextScene());
        }
    }

    public void MainMenuScene()
    {
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void ReloadScene()
    {
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        IsGamePaused = false;
        SceneMusicVolume.value = 0.5f;
        SceneSfxVolume.value = 0.5f;
        SetVolume();
    }

    public IEnumerator NextScene()
    {
        PlayerMovementControl.Instance.PlayerWin();
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
