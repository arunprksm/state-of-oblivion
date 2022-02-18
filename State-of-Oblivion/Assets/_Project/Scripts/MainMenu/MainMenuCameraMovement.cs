using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuCameraMovement : MonoBehaviour
{
    [SerializeField] private float mainCameraSpeed;
    [SerializeField] private GameObject optionMenu;
    private SoundManager soundManager;
    //[SerializeField] internal GameObject playMenu;
    private void Start()
    {
        soundManager = SoundManager.Instance;
        soundManager.PlayMusic(Sounds.Music);
        //SoundManager.Instance.PlayMusic(Sounds.Music);

        //optionMenu = GameObject.FindGameObjectWithTag("OptionMenu");
        optionMenu = SoundManager.Instance.optionMenu;
        optionMenu.SetActive(false);
    }
    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * mainCameraSpeed);
    }
    public void PlayButton()
    {
        //playMenu.SetActive(false);
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
