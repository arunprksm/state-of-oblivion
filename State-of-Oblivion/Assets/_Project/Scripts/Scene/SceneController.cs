using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static bool IsGamePaused = false;
    [SerializeField] private GameObject pauseMenuPanel;

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
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
    }
    
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
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
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    public void ReloadScene()
    {
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
