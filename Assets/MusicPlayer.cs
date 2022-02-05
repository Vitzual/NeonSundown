using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Audio source for music
    public List<AudioClip> tracks;
    private AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        // Get the audio source
        music = GetComponent<AudioSource>();
        music.clip = tracks[Random.Range(0, tracks.Count)];
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!music.isPlaying)
        {
            AudioClip clip = tracks[Random.Range(0, tracks.Count)];
            if (clip != music.clip)
            {
                music.clip = tracks[Random.Range(0, tracks.Count)];
                music.Play();
            }
        }
    }
}
