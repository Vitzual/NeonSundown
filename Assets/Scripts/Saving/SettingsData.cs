using System;

[Serializable]
public class SettingsData
{
    // Audio options
    public float music;
    public float sound;

    // Other options
    public int resolutionX;
    public int resolutionY;
    public bool screenmode;
    public int framerate;
    public bool screenShake;
    public float glowAmount;
    public float lightAmount;
    public bool shipColoring;
    public bool useParticles;
    public bool fastCardAnim;
    public bool skipCardAnim;
    public bool musicPitching;

    // Keybinds
    public string keybind_move_up;
    public string keybind_move_left;
    public string keybind_move_down;
    public string keybind_move_right;
    public string keybind_dash;
    public string keybind_primary;
    public string keybind_secondary;
    public string keybind_escape;
    public string keybind_debug;
}
