using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    public float timeRemaining = 17;
    public bool timerIsRunning = false;
    public TextMeshProUGUI timeText;
    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        StartCoroutine(WaitForGameEnd());
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        int t = (int)timeToDisplay;
        timeText.text =  "Game Ends in: " + t.ToString() + " Secs";
    }

    IEnumerator WaitForGameEnd()
    {
        yield return new WaitForSecondsRealtime(timeRemaining);
        sceneController.MainMenuScene();
    }
}
