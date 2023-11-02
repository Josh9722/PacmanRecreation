using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float elapsedTime;
    private bool isCountingDown;
    private bool isPaused;

    public Timer(float initialTime, bool countDown = true)
    {
        elapsedTime = initialTime;
        isCountingDown = countDown;
        isPaused = true;
    }

    public void Start() { 
        isPaused = false;
    }

    public void Update(float deltaTime)
    {
        if (isPaused || IsFinished()) {
            return; 
        }
        
            if (isCountingDown) { 
                elapsedTime -= deltaTime;
            } else {
                elapsedTime += deltaTime;
            }
 
        IsFinished();
    }

    public float GetTime()
    {
        return elapsedTime;
    }

    public bool IsFinished()
    {
        if (isCountingDown)
        {
            if (elapsedTime <= 0f) { 
                elapsedTime = 0f;
                return true;
            }
        }
        return false;
    }

    public void Reset()
    {
        elapsedTime = 0f;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
    }

    public override string ToString()
    {
        return $"ElapsedTime: {elapsedTime}, CountingDown: {isCountingDown}, Paused: {isPaused}";
    }
}


