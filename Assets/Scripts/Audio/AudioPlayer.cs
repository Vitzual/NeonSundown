using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Button sound
    public AudioClip _buttonSound;
    public AudioClip _stickySound;

    // List of audio sources
    public static List<AudioClip> audioClips;
    public static List<float> cooldowns;
    public static AudioSource audioSource;
    public static AudioClip buttonSound;
    public static AudioClip stickySound;

    // Set audio sources
    public void Awake()
    {
        buttonSound = _buttonSound;
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
    public static void Play(AudioClip clip, bool randomizePitch = true, float minPitch = 0.9f, 
        float maxPitch = 1.1f, bool overrideCooldown = false, float audioScale = 1)
    {
        // Check if cooldown expired
        if (!overrideCooldown)
        {
            if (audioClips.Contains(clip)) return;
            else
            {
                audioClips.Add(clip);
                cooldowns.Add(0.1f);
            }
        }

        // Set volume and pitch
        audioSource.volume = Settings.sound;
        if (randomizePitch) audioSource.pitch = Random.Range(minPitch, maxPitch);
        else audioSource.pitch = 1f;

        audioSource.PlayOneShot(clip, audioScale);
    }

    // Plays a button sound on hover
    public static void PlayButton()
    {
        if (buttonSound != null)
        {
            audioSource.volume = Settings.sound;
            audioSource.PlayOneShot(buttonSound);
        }
    }

    // Plays a button sound on hover
    public static void PlayStickySound()
    {
        if (stickySound != null)
        {
            if (audioClips.Contains(stickySound)) return;
            else
            {
                audioClips.Add(stickySound);
                cooldowns.Add(0.1f);
            }

            audioSource.volume = Settings.sound;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(stickySound);
        }
    }
}
