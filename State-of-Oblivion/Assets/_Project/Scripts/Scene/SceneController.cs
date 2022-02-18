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
        CurrentVolume();
    }
    private void CurrentVolume()
    {
        SceneMusicVolume.value = GameManager.Instance.currentMusicVolume;
        SceneSfxVolume.value = GameManager.Instance.currentSfxVolume;
    }
    private void SetVolume()
    {
        GameManager.Instance.currentMusicVolume = SceneMusicVolume.value;
        GameManager.Instance.currentSfxVolume = SceneSfxVolume.value;
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
        SetVolume();
    }
    
    public void Resume()
    {
        SoundManager.Instance.PlayMusic(Sounds.GameMusic_scene1);
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    private void Pause()
    {
        SoundManager.Instance.PauseMusic(Sounds.GameMusic_scene1);
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
        //mainMenuCameraMovement.playMenu.SetActive(true);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void ReloadScene()
    {
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public IEnumerator NextScene()
    {
        PlayerMovementControl.Instance.PlayerWin();
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
