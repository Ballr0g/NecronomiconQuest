using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFX : MonoBehaviour
{
    [Header("Footsteps WAV")]
    public AudioClip Footstep_SFX;
    [Header("Jump WAV")]
    public AudioClip Jump_SFX;
    [Header("Audio Source Component")]
    public AudioSource Audio_Source;
    public float walkVolume = 1.0f;

    void Start()
    {
        Audio_Source = GetComponent<AudioSource>();
    }

    public void Playfootsteps()
    {
        Audio_Source.pitch = Random.Range(0.8f, 1.2f);
        Audio_Source.volume = walkVolume;
        Audio_Source.PlayOneShot(Footstep_SFX);
    }

    public void PlayJumpSound()
    {
        Audio_Source.pitch = Random.Range(0.8f, 1.2f);
        Audio_Source.PlayOneShot(Jump_SFX);
    }
}
