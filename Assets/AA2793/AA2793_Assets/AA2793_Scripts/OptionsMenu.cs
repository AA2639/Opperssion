using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public Toggle fullscreenToggle, vsyncToggle;

    public Resolution[] resolutions;
    public int selectedResolution = 1;
    public Text resolutionLabel;

    public AudioMixer mixer;

    public Slider masterSlider, backgroundMusicSlider, sfxSlider;
    public Text masterLabel, backgroundMusicLabel, sfxLabel;

    public AudioSource sfxLoop;


    // Start is called before the first frame update
    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncToggle.isOn = false;
        }
        else
        {
            vsyncToggle.isOn = true;
        }

        bool resolutionFound = false;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical )
            {
                resolutionFound = true;

                selectedResolution = i;

                UpdateResolutionLabel();
            }
        }

        if (!resolutionFound)
        {
            resolutionLabel.text = Screen.width.ToString() + " x " + Screen.height.ToString();
        }


        //Checking if theres data saved for the volume
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            
        }

        if (PlayerPrefs.HasKey("BackgroundMusicVolume"))
        {
            mixer.SetFloat("BackgroundMusicVolume", PlayerPrefs.GetFloat("BackgroundMusicVolume"));
            backgroundMusicSlider.value = PlayerPrefs.GetFloat("BackgroundMusicVolume");
            
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
            
        }

        masterLabel.text = (masterSlider.value + 80).ToString();
        backgroundMusicLabel.text = (backgroundMusicSlider.value + 80).ToString();
        sfxLabel.text = (sfxSlider.value + 80).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResolutionMore()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResolutionLabel();
    }

    public void ResolutionLess()
    {
        selectedResolution++;

        if (selectedResolution > resolutions.Length - 1)
        {
            selectedResolution = resolutions.Length - 1;
        }

        UpdateResolutionLabel();
    }

    public void UpdateResolutionLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphicsChanges()
    {
        //Apply fullscreen
        //Screen.fullScreen = fullscreenToggle.isOn;

        if (vsyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        //setting resolution
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    }

    public void SetMasterVolume()
    {
        masterLabel.text = (masterSlider.value + 80).ToString();
        mixer.SetFloat("MasterVolume", masterSlider.value);

        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        backgroundMusicLabel.text = (backgroundMusicSlider.value + 80).ToString();
        mixer.SetFloat("BackgroundMusicVolume", backgroundMusicSlider.value);

        PlayerPrefs.SetFloat("BackgroundMusicVolume", backgroundMusicSlider.value);
    }

    public void SetSFXVolume()
    {
        sfxLabel.text = (sfxSlider.value + 80).ToString();
        mixer.SetFloat("SFXVolume", sfxSlider.value);

        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void PlaySFXLoop()
    {
        sfxLoop.Play();
    }

    public void StopSFXLoop()
    {
        sfxLoop.Stop();
    }
}

[System.Serializable]
public class Resolution
{
    public int horizontal, vertical;

}
