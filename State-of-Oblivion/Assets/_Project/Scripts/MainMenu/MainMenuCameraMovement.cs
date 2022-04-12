using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuCameraMovement : MonoBehaviour
{
    [SerializeField] private float mainCameraSpeed;
    [SerializeField] private GameObject optionMenu;
    private void Start()
    {
        SoundManager.Instance.PlayMusic(Sounds.Music);
        optionMenu = SoundManager.Instance.optionMenu;
        optionMenu.SetActive(false);
    }
    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * mainCameraSpeed);
    }
    public void PlayButton()
    {
        optionMenu.SetActive(false);
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}
