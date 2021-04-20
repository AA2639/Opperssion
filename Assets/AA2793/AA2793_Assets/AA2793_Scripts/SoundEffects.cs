using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    private AudioSource audioSource;


    private void Awake()
    {
        //Assigning animator to anim

        audioSource = GetComponent<AudioSource>();

    }

    //Animation Alex_Run_Back
    private void StepBackSound()
    {
        Debug.Log("moving forward");
        AudioClip clip = clips[0];
        audioSource.PlayOneShot(clip);
    }

    //Animation Alex_Run
    private void StepForwardSound()
    {
        AudioClip clip = clips[1];
        audioSource.PlayOneShot(clip);
    }

    //Animation Alex_FiringRifle
    private void FiringSound()
    {
        AudioClip clip = clips[2];
        audioSource.PlayOneShot(clip);
    }

    //Animation AlexForwardJumpUpEditable and AlexJumpUPEditable
    private void JumpingSound()
    {
        Debug.Log("im jumping");
        AudioClip clip = clips[3];
        audioSource.PlayOneShot(clip);
    }

    //Not yet implemented
    //private void JumpChargeSound()
    //{
    //    Debug.Log("playing delay");
    //    audioSource.PlayDelayed(delay);
    //    Debug.Log(audioSource);
    //}


}