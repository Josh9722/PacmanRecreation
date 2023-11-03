using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource effectsAudioSource;
    public AudioClip normalGhostMusic;
    public AudioClip scaredGhostMusic;
    public AudioClip deadGhostMusic;
    public AudioClip pacStudentMove;
    public AudioClip pelletEaten;
    public AudioClip pacStudentCollidesWall;
    public AudioClip pacStudentDeath;

    void Start()
    {
        
    }

    public void PlayNormalGhostMusic()
    {
        musicAudioSource.clip = normalGhostMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayScaredGhostMusic()
    {
        musicAudioSource.clip = scaredGhostMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayDeadGhostMusic()
    {
        musicAudioSource.clip = deadGhostMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlayPacStudentMove()
    {
        effectsAudioSource.clip = pacStudentMove;
        effectsAudioSource.loop = false;
        effectsAudioSource.Play();
    }

    public void PlayPelletEaten()
    {
        effectsAudioSource.clip = pelletEaten;
        effectsAudioSource.loop = false;
        effectsAudioSource.Play();
    }

    public void PlayPacStudentCollidesWall()
    {
        effectsAudioSource.clip = pacStudentCollidesWall;
        effectsAudioSource.loop = false;
        effectsAudioSource.Play();
    }

    public void PlayPacStudentDeath()
    {
        musicAudioSource.clip = pacStudentDeath;
        musicAudioSource.loop = false;
        musicAudioSource.Play();
    }

    



}
