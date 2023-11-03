using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class StartSceneManager : MonoBehaviour
{
    public TextMeshProUGUI highScoreText; 
    public TextMeshProUGUI highScoreTimeText;


    // Start is called before the first frame update
    void Start()
    {
        LoadHighScore();

    }

    private void LoadHighScore()
    {
        // Load and update high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = $"High Score: {highScore}";

        // Load and update high score time
        float highScoreTime = PlayerPrefs.GetFloat("HighScoreTime", float.MaxValue);
        highScoreTimeText.text = highScoreTime < float.MaxValue ? $"Best Time: {FormatTime(highScoreTime)}" : "Best Time: --:--";
    }

    private string FormatTime(float time)
    {
        // Format the time in minutes and seconds
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }

    public void onLevelOneButtonPressed()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level01");
    }
}
