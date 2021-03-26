using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;

    private float delay = 2f;

    //Sound effect functions
    private void StepBackSound()
    {
        AudioClip clip = clips[0];
        audioSource.PlayOneShot(clip);
    }
    private void StepForwardSound()
    {
        AudioClip clip = clips[1];
        audioSource.PlayOneShot(clip);
    }


    private void Firing()
    {
        AudioClip clip = clips[2];
        audioSource.PlayOneShot(clip);
    }

    //private void Jump()
    //{
    //    AudioClip clip = clips[2];
    //    audioSource.PlayOneShot(clip);
    //}

    private void JumpCharge()
    {
        Debug.Log("playing delay");
        audioSource.PlayDelayed(delay);
        Debug.Log(audioSource);
    }


    private void Awake()
    {
        //Assigning animator to anim
            
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)           
        {
            Debug.Log("im in");
            AudioClip clip = clips[0];
            audioSource.PlayOneShot(clip);
        }
    }
}


