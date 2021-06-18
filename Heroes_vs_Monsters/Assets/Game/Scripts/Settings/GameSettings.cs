using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    float currentVolume;
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        // Load the settings
        LoadSettings();

        // If there is music
        if(music != null)
        {
            // Play it after some time
            music.PlayDelayed(0.1f);
        }    
    }

    public void SetVolume()
    {
        // Get the slider value
        float sliderValue = volumeSlider.value;

        // Set the initial volume value
        float volume = -10;

        // Change the volume value (using the slider value)
        switch (sliderValue)
        {
            case 0:
                volume = -80;
                break;
            case 1:
                volume = -30;
                break;
            case 2:
                volume = -20;
                break;
            case 3:
                volume = -15;
                break;
            case 4:
                volume = -10;
                break;
            case 5:
                volume = -5;
                break;
            case 6:
                volume = 0;
                break;
            case 7:
                volume = 5;
                break;
            case 8:
                volume = 8;
                break;
            case 9:
                volume = 10;
                break;
            case 10:
                volume = 12;
                break;
            case 11:
                volume = 15;
                break;
            default:
                volume = -10;
                break;
        }

        // Set the new volume value
        audioMixer.SetFloat("Music", volume);

        // Update the current value
        currentVolume = volume;

        // Save new settings
        SaveSettings();
    }

    public void SaveSettings()
    {
        // Change player preferences
        PlayerPrefs.SetFloat("VolumePreference",
                   volumeSlider.value);
    }

    public void LoadSettings()
    {
        // If there is a VolumePreference key in the player preferences
        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            // Get the volume value
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        }    
        else volumeSlider.value = 4; // If that key does not exit, set a default volume
    }

    public float GetPreferredVolume()
    {
        // Return current volume
        return currentVolume;
    }
}
