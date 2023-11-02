using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public GameObject exitButton; 
    public GameObject Score; 
    public GameObject gameTimer;
    public GameObject ghostTimer; 
    public GameObject Lives;

    // Timers
    private Timer gameTimerTimer;
    private Timer ghostTimerTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Counts upward to infinity 
        gameTimerTimer = new Timer(0.0f, countDown: false);
        gameTimerTimer.Start();

        // Counts down from 20 seconds
        ghostTimerTimer = new Timer(20.0f, countDown: true);    
    }

    // Update is called once per frame
    void Update()
    {
        gameTimerTimer.Update(Time.deltaTime);
        ghostTimerTimer.Update(Time.deltaTime);

        setGameTimerText();
        setGhostTimerText();
    }

    private void setGameTimerText() {
        TextMeshProUGUI gameTimerText = gameTimer.GetComponentInChildren<TextMeshProUGUI>();
        float elapsedTime = gameTimerTimer.GetTime();

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

        string timerText = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        gameTimerText.text = timerText;
    }

    private void setGhostTimerText() {
        // if ghostTimer gameobject is enabled
        if (!ghostTimer.activeSelf) {
            return;
        }
        
        TextMeshProUGUI ghostTimerText = ghostTimer.GetComponentInChildren<TextMeshProUGUI>();
        float elapsedTime = ghostTimerTimer.GetTime();
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        ghostTimerText.text = seconds.ToString();
    }

    public void enableGhostTimer() { 
        ghostTimer.SetActive(true);
        ghostTimerTimer.Start();

    }
}
