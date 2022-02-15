using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoader : MonoBehaviour
{
    // Audio source for music
    public List<AudioClip> _tracks;
    public static List<AudioClip> tracks;
    private static bool clipsLoaded = false;

    // Load clips on startup
    private void Start()
    {
        if (!clipsLoaded)
        {
            tracks = new List<AudioClip>(_tracks);
            clipsLoaded = true;
        }
    }
}
