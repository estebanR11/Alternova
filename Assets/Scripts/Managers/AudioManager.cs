using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [Header("Music")]
    [SerializeField] AudioClip startMusic;
    [SerializeField] AudioClip gameMusic;
    [SerializeField] AudioClip endMusic;

    [Header("SFX")]
    [SerializeField] AudioClip correctSound;
    [SerializeField] AudioClip incorrectSound;

    private void Start()
    {
        MainMenuMusic();
    }
    public void MainMenuMusic()
    {
        musicAudioSource.clip = startMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();        
    }

    public void GameMusic()
    {
        musicAudioSource.clip = gameMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();        
    }

    public void EndMusic()
    {
        musicAudioSource.clip = endMusic;
        musicAudioSource.loop = true;
        musicAudioSource.Play();        
    }

    public void CorrectSound()
    {
        sfxAudioSource.clip = correctSound;
        sfxAudioSource.Play();
    }

    public void IncorrectSound()
    {
        sfxAudioSource.clip = incorrectSound;
        sfxAudioSource.Play();
    }
}
