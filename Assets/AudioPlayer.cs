using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // List of audio sources
    public static AudioSource audioSource;

    // Set audio sources
    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Play an audio clip
    public static void Play(AudioClip clip, bool randomizePitch = true)
    {
        if (randomizePitch)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }
        else audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip, Settings.sound);
    }
}
