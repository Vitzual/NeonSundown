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

    // Keybinds
    public string keybind_move_up;
    public string keybind_move_left;
    public string keybind_move_down;
    public string keybind_move_right;
    public string keybind_dash;
    public string keybind_shoot;
    public string keybind_ability;
    public string keybind_escape;
    public string keybind_debug;
}
