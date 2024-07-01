using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para usar SceneManager

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundSource;

    public AudioClip background;
    public AudioClip background2;
    public AudioClip cinematic;
    public AudioClip shot;
    public AudioClip enemyDeath;

    private void Start()
    {
        SetBackgroundMusic();
    }

    private void SetBackgroundMusic()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Menu":
                musicSource.clip = background;
                break;
            case "GameScene":
                musicSource.clip = background;
                break;
            case "Zone2":
                musicSource.clip = background2;
                break;
            case "Cinematics":
                musicSource.clip = cinematic;
                break;
            default:
                musicSource.clip = background; // Música por defecto
                break;
        }

        musicSource.Play();
    }

    public void playSound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }
}

