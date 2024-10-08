using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI; 
public class SliderVolume : MonoBehaviour
{
    [Header("Music")]
    public Slider musicSlider;
    public float musicVolume;

    [Header("Sound")]
    public Slider soundSlider;
    public float soundVolume;

    public AudioManager audioManager;

    private void Start()
    {
        
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        audioManager.SetMusicVolume(musicSlider.value);

       
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.6f);
        audioManager.SetSoundVolume(soundSlider.value);

        // Asignar los eventos de cambio de slider
        musicSlider.onValueChanged.AddListener(ChangeMusicSlider);
        soundSlider.onValueChanged.AddListener(ChangeSoundSlider);
    }

    public void ChangeMusicSlider(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        audioManager.SetMusicVolume(musicVolume);
    }

    public void ChangeSoundSlider(float value)
    {
        soundVolume = value;
        PlayerPrefs.SetFloat("SoundVolume", soundVolume);
        audioManager.SetSoundVolume(soundVolume);
    }
}
