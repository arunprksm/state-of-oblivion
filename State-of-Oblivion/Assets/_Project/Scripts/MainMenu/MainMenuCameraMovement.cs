using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuCameraMovement : MonoBehaviour
{
    [SerializeField] private float mainCameraSpeed;

    private void Start()
    {
        SoundManager.Instance.PlayMusic(Sounds.Music);
    }
    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * mainCameraSpeed);
    }
    public void PlayButton()
    {
        SoundManager.Instance.PlaySFX(Sounds.ButtonClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
