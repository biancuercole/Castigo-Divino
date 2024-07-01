using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundSource;

    [SerializeField] AudioClip background;
    [SerializeField] public AudioClip shot;
    [SerializeField] public AudioClip enemyDeath;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void playSound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }

}
