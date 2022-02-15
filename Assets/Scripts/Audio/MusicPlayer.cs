using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Audio source for music
    public bool useRandomTracks = false;
    private bool arenaMusic = false;
    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        // Get the audio source
        music = GetComponent<AudioSource>();
        if (Gamemode.arena.arenaMusic == null)
        {
            music.clip = MusicLoader.tracks[Random.Range(0, MusicLoader.tracks.Count)];
        }
        else
        {
            music.clip = Gamemode.arena.arenaMusic;
            music.loop = true;
            arenaMusic = true;
        }

        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!arenaMusic && !music.isPlaying)
        {
            AudioClip clip = MusicLoader.tracks[Random.Range(0, MusicLoader.tracks.Count)];
            if (clip != music.clip)
            {
                music.clip = MusicLoader.tracks[Random.Range(0, MusicLoader.tracks.Count)];
                music.Play();
            }
        }
    }
}
