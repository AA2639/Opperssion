using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AuidioManager : MonoBehaviour
{
    public AudioMixer mixer;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume")); 
        }

        if (PlayerPrefs.HasKey("BackgroundMusicVolume"))
        {
            mixer.SetFloat("BackgroundMusicVolume", PlayerPrefs.GetFloat("BackgroundMusicVolume"));
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
