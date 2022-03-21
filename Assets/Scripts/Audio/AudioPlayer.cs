using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    // Button sound
    public AudioClip _buttonSound;
    public AudioClip _stickySound;
    public AudioClip _reflectSound;

    // List of audio sources
    public static List<AudioClip> audioClips;
    public static List<float> cooldowns;
    public static AudioSource audioSource;
    public static AudioClip buttonSound;
    public static AudioClip critSound;
    public static AudioClip reflectSound;

    // Set audio sources
    public void Awake()
    {
        buttonSound = _buttonSound;
        critSound = _stickySound;
        reflectSound = _reflectSound;
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
            audioSource.PlayOneShot(buttonSound, 0.5f);
        }
    }

    // Plays a button sound on hover
    public static void PlayCritSound()
    {
        if (audioClips.Contains(critSound)) return;
        else
        {
            audioClips.Add(critSound);
            cooldowns.Add(0.1f);
        }

        audioSource.volume = Settings.sound;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(critSound, 0.4f);
    }

    // Plays a button sound on hover
    public static void PlayReflectSound()
    {
        if (audioClips.Contains(reflectSound)) return;
        else
        {
            audioClips.Add(reflectSound);
            cooldowns.Add(0.1f);
        }

        audioSource.volume = Settings.sound;
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(reflectSound, 0.4f);
    }
}
