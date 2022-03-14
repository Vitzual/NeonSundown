using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Audio source for music
    public bool useRandomTracks = false;
    public bool _isMenu = false;
    public AudioSource _bossMusic;
    public static bool isMenu = false;
    public static AudioSource music;
    private static AudioSource bossMusic;

    // Fade in variables
    private static float targetFadeIn = 1f;
    private static float targetFadeOut = 0.5f;

    // Start is called before the first frame update
    public void Start()
    {
        // Subscribe to volume change event
        Events.active.onVolumeChanged += UpdateVolume;
        Events.active.onMusicPitchChanged += ResetPitch;

        // Get the audio source
        music = GetComponent<AudioSource>();
        bossMusic = _bossMusic;
        isMenu = _isMenu;

        // Check if is menu
        if (!_isMenu)
        {
            music.clip = Gamemode.arena.arenaMusic;
            music.loop = true;
        }

        if(bossMusic != null) bossMusic.volume = Settings.sound;
        music.volume = Settings.music;
        music.Play();
    }

    // Update music volume
    public void UpdateVolume(float volume)
    {
        music.volume = volume;
        if (bossMusic != null) 
            bossMusic.volume = Settings.sound;
        StopAllCoroutines();
    }

    // Play / Stop the music
    public static void PlayMusic() { music.Play(); }
    public static void StopMusic() { music.Pause(); }
    public static void ResetPitch() { music.pitch = 1f; }
    
    // Play boss music
    public static void PlayBossMusic()
    {
        if (bossMusic != null)
        {
            music.Pause();
            bossMusic.Play();
        }
    }

    // Stop boss music
    public static void StopBossMusic()
    {
        if (bossMusic != null)
        {
            music.Play();
            bossMusic.Stop();
        }
    }

    // Creates a fade in
    public static IEnumerator FadeIn(float FadeTime, float delay)
    {
        float startVolume = music.volume;
        targetFadeIn = Settings.music;

        while (delay > 0f)
        {
            delay -= Time.deltaTime;
            yield return null;
        }

        while (music.volume < targetFadeIn)
        {
            music.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        music.volume = targetFadeIn;
    }

    // Creates a fade out
    public static IEnumerator FadeOut(float FadeTime, float divisor)
    {
        float startVolume = music.volume;
        targetFadeOut = Settings.music / divisor;

        while (music.volume > targetFadeOut)
        {
            music.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        music.volume = targetFadeOut;
    }
}
