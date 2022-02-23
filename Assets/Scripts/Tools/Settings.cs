using System.IO;
using UnityEngine;

public class Settings
{
    // Settings file location
    private const string SETTINGS_PATH = "/settings.json";

    // Volume enum
    public enum Volume
    {
        Master,
        Music,
        Sound
    }

    // Volume control
    public static float music = 0.8f;
    public static float sound = 0.5f;

    // Video options
    private static int resolutionX = 1920;
    private static int resolutionY = 1080;
    private static bool screenmode = true;
    private static int framerate = 999;
    public static bool screenShake = true;

    // Save settings
    public static void SaveSettings()
    {
        // Create new save data instance
        SettingsData settingsData = new SettingsData();

        // Set settings data
        settingsData.music = music;
        settingsData.sound = sound;
        settingsData.resolutionX = resolutionX;
        settingsData.resolutionY = resolutionY;
        settingsData.screenmode = screenmode;
        settingsData.framerate = framerate;
        settingsData.screenShake = screenShake;

        // Get keybinds from file
        settingsData.keybind_move_up = Keybinds.move_up.ToString();
        settingsData.keybind_move_left = Keybinds.move_left.ToString();
        settingsData.keybind_move_down = Keybinds.move_down.ToString();
        settingsData.keybind_move_right = Keybinds.move_right.ToString();
        settingsData.keybind_dash = Keybinds.dash.ToString();
        settingsData.keybind_shoot = Keybinds.shoot.ToString();
        settingsData.keybind_escape = Keybinds.escape.ToString();

        // Convert to json and save
        string data = JsonUtility.ToJson(settingsData);
        File.WriteAllText(Application.persistentDataPath + SETTINGS_PATH, data);
    }

    // Load settings
    public static void LoadSettings()
    {
        // Get path
        string path = Application.persistentDataPath + SETTINGS_PATH;

        // Check if file exists
        if (File.Exists(path))
        {
            // Load json file
            string data = File.ReadAllText(path);
            SettingsData settingsData = JsonUtility.FromJson<SettingsData>(data);

            // Parse settings data from file
            music = settingsData.music;
            sound = settingsData.sound;
            resolutionX = settingsData.resolutionX;
            resolutionY = settingsData.resolutionY;
            screenmode = settingsData.screenmode;
            framerate = settingsData.framerate;
            screenShake = settingsData.screenShake;

            // Get keybinds from file
            Keybinds.move_up = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_up);
            Keybinds.move_left = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_left);
            Keybinds.move_down = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_down);
            Keybinds.move_right = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_right);
            Keybinds.dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_dash);
            Keybinds.shoot = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_shoot);
            Keybinds.escape = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_escape);

            // Apply settings
            UpdateVideoSettings();
            UpdateVolumeSettings();
        }
        else
        {
            // Set default settings
            Keybinds.SetDefaultKeybinds();
            music = 0.5f;
            sound = 1f;
            resolutionX = 1920;
            resolutionY = 1080;
            screenmode = true;
            framerate = 999;
        }
    }

    // Set the volume
    public static void SetVolume(Volume type, float value)
    {
        // Set the new volume
        switch(type)
        {
            case Volume.Music:
                music = value;
                break;
            case Volume.Sound:
                sound = value;
                break;
        }

        // Update volume for all components
        UpdateVolumeSettings();
    }

    // Set video componenets
    public static void SetScreenMode(bool toggle)
    { 
        screenmode = toggle;
        UpdateVideoSettings();
    }

    public static void SetResolution(int x, int y)
    { 
        resolutionX = x; 
        resolutionY = y;
        UpdateVideoSettings();
    }

    // Set the framerate
    public static void SetFramerate(int amount)
    { 
        if (amount >= 30) 
            framerate = amount;
        UpdateVideoSettings();
    }

    // Apply settings
    public static void UpdateVideoSettings()
    {
        Screen.SetResolution(resolutionX, resolutionY, screenmode, framerate);
        Application.targetFrameRate = framerate;
    }

    // Update the volume
    public static void UpdateVolumeSettings()
    {
        if (Events.active != null)
            Events.active.VolumeChanged(music);
    }
}
