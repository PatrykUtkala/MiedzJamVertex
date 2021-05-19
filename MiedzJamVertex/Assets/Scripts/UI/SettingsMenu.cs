using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    Resolution[] resolutions;
    public Dropdown ResolutionDropdown;
    public Slider VolumeSlider;
    public Toggle FullScreenToggle;
    //public Dropdown GraphicsDropdown;

    private void Start()
    {
        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string Option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(Option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();

        VolumeSlider.value = PlayerPrefs.GetFloat("volume", 0);
        FullScreenToggle.isOn = Screen.fullScreen;
        //GraphicsDropdown.value = PlayerPrefs.GetInt("qualityIndex", 5);
    }

    public void SetResolution(int ResolutionIndex)
    {
        resolutions = Screen.resolutions;
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);   
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        AudioMixer.SetFloat("volume", volume);
    }


    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("qualityIndex", qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex);
    }


    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
