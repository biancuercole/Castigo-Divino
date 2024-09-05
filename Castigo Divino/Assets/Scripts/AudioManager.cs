using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundSource;

    public AudioClip background;
    public AudioClip background2;
    public AudioClip ambientSound;
    public AudioClip cinematic;
    public AudioClip shot;
    public AudioClip steps;
    public AudioClip enemyDeath;
    public AudioClip victory;
    public AudioClip gameOver;
    public AudioClip enemyShot;
    public AudioClip openDoor;
    public AudioClip bossMusic;
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
            case "PacificZone":
                musicSource.clip = background2;
                break;
            case "Cinematics":
                musicSource.clip = cinematic;
                break;
            case "Victory":
                soundSource.clip = victory;
                break;
            case "GameOver":
                soundSource.clip = gameOver;
                break;
            case "EnemyLevel":
            soundSource.clip = bossMusic;
            break;
            /*default:
                musicSource.clip = background; // Música por defecto
                break;*/
        }
        soundSource.Play();
        musicSource.Play();
    }

    public void playSound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

    public void ChangeBackgroundMusic(AudioClip newMusic)
    {
        // Cambia el clip y reproduce la nueva música
        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.Play();
    }

}

