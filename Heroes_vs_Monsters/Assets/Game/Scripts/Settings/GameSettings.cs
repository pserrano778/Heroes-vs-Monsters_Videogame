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
    public Toggle fullScreenToggle;

    // Start is called before the first frame update
    void Start()
    {
        LoadSettings();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

      

    public void SetVolume()
    {
        float sliderValue = volumeSlider.value;
        float volume = -10;

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

        audioMixer.SetFloat("Music", volume);
        currentVolume = volume;
        SaveSettings();
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = fullScreenToggle.isOn;
        print("fullScreen " + fullScreenToggle.isOn);
        SaveSettings();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("FullscreenPreference",
                   Convert.ToInt32(fullScreenToggle.isOn));
        PlayerPrefs.SetFloat("VolumePreference",
                   volumeSlider.value);
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            fullScreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }  
        else fullScreenToggle.isOn = true;


        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        }    
        else volumeSlider.value = 4;
    }

    public float GetPreferredVolume()
    {
        return currentVolume;
    }

}
