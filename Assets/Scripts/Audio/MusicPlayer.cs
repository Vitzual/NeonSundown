using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Audio source for music
    public bool useRandomTracks = false;
    public bool isMenu = false;
    private bool arenaMusic = false;
    private static AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to volume change event
        Events.active.onVolumeChanged += UpdateVolume;

        // Get the audio source
        music = GetComponent<AudioSource>();

        // Check if is menu
        if (!isMenu)
        {
            music.clip = Gamemode.arena.arenaMusic;
            music.loop = true;
            arenaMusic = true;
        }

        music.volume = Settings.music;
        music.Play();
    }

    // Update music volume
    public void UpdateVolume(float volume)
    {
        music.volume = volume;
    }

    // Play / Stop the music
    public static void PlayMusic() { music.Play(); }
    public static void StopMusic() { music.Pause(); }
}
