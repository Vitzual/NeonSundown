using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // List of audio sources
    public static List<AudioClip> audioClips;
    public static List<float> cooldowns;
    public static AudioSource audioSource;

    // Set audio sources
    public void Awake()
    {
        audioClips = new List<AudioClip>();
        cooldowns = new List<float>();
        audioSource = GetComponent<AudioSource>();
    }

    // Keeps an update list of audio clip cooldowns
    public void Update()
    {
        // Update cooldowns
        for(int i = 0; i < audioClips.Count; i++)
        {
            if (cooldowns[i] <= 0f)
            {
                audioClips.RemoveAt(i);
                cooldowns.RemoveAt(i);
                i--;
            }
            else cooldowns[i] -= Time.deltaTime;
        }
    }

    // Play an audio clip
    public static void Play(AudioClip clip, bool randomizePitch = true)
    {
        // Check if cooldown expired
        if (audioClips.Contains(clip)) return;
        else
        {
            audioClips.Add(clip);
            cooldowns.Add(0.1f);
        }

        if (randomizePitch) audioSource.pitch = Random.Range(0.9f, 1.1f);
        else audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip, Settings.sound);
    }
}
