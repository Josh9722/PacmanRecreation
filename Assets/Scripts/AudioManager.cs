using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;  
    public AudioClip introMusic;
    public AudioClip normalGhostMusic;
    public AudioClip scaredGhostMusic;
    public AudioClip deadGhostMusic;
    public AudioClip pacStudentMove;
    public AudioClip pelletEaten;
    public AudioClip pacStudentCollidesWall;
    public AudioClip pacStudentDeath;

    void Start()
    {
        PlayIntroMusic();   
    }

    public void PlayIntroMusic()
    {
        audioSource.loop = false;
        audioSource.clip = introMusic;
        audioSource.Play();
        
        //When the intro music finishes, play the normal ghost music
        Invoke("PlayNormalGhostMusic", introMusic.length);
    }

    public void PlayNormalGhostMusic()
    {
        audioSource.clip = normalGhostMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayScaredGhostMusic()
    {
        audioSource.clip = scaredGhostMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayDeadGhostMusic()
    {
        audioSource.clip = deadGhostMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayPacStudentMove()
    {
        audioSource.clip = pacStudentMove;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayPelletEaten()
    {
        audioSource.clip = pelletEaten;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayPacStudentCollidesWall()
    {
        audioSource.clip = pacStudentCollidesWall;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayPacStudentDeath()
    {
        audioSource.clip = pacStudentDeath;
        audioSource.loop = false;
        audioSource.Play();
    }

    



}
