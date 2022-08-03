using System.IO;
using UnityEngine;

public class Settings
{
    // Settings file location
    private const string SETTINGS_PATH = "/settings_save.json";

    // Volume enum
    public enum Volume
    {
        Master,
        Music,
        Sound
    }

    // Volume control
    public static float music = 1f;
    public static float sound = 1f;

    // Video options
    private static int resolutionX = 1920;
    private static int resolutionY = 1080;
    private static bool screenmode = true;
    private static int framerate = 999;
    public static bool shipColoring = true;
    public static bool screenShake = true;
    public static bool useParticles = true;
    public static bool alwaysShowHP = false;
    public static float glowAmount = 1f;
    public static float lightAmount = 1f;
    public static bool damageNumbers = false;
    public static bool compoundXP = false;
    public static bool musicPitching = true;
    public static bool verticalsync = false;
    public static bool controllerInput = true;
    public static bool xpTrails = true;

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
        settingsData.shipColoring = shipColoring;
        settingsData.screenShake = screenShake;
        settingsData.glowAmount = glowAmount;
        settingsData.lightAmount = lightAmount;
        settingsData.useParticles = useParticles;
        settingsData.alwaysShowHP = alwaysShowHP;
        settingsData.damageNumbers = damageNumbers;
        settingsData.compoundXP = compoundXP;
        settingsData.musicPitching = musicPitching;
        settingsData.verticalsync = verticalsync;
        settingsData.controllerInput = controllerInput;
        settingsData.xpTrails = xpTrails;

        // Get keybinds from file
        settingsData.keybind_move_up = Keybinds.move_up.ToString();
        settingsData.keybind_move_left = Keybinds.move_left.ToString();
        settingsData.keybind_move_down = Keybinds.move_down.ToString();
        settingsData.keybind_move_right = Keybinds.move_right.ToString();
        settingsData.keybind_dash = Keybinds.dash.ToString();
        settingsData.keybind_primary = Keybinds.primary.ToString();
        settingsData.keybind_secondary = Keybinds.secondary.ToString();
        settingsData.keybind_burn = Keybinds.burn.ToString();
        settingsData.keybind_escape = Keybinds.escape.ToString();
        settingsData.keybind_stats = Keybinds.stats.ToString();
        settingsData.keybind_autofire = Keybinds.autofire.ToString();

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
            shipColoring = settingsData.shipColoring;
            screenShake = settingsData.screenShake;
            useParticles = settingsData.useParticles;
            alwaysShowHP = settingsData.alwaysShowHP;
            damageNumbers = settingsData.damageNumbers;
            compoundXP = settingsData.compoundXP;
            musicPitching = settingsData.musicPitching;
            verticalsync = settingsData.verticalsync;
            controllerInput = settingsData.controllerInput;
            xpTrails = settingsData.xpTrails;

            // Apply glow effect
            glowAmount = settingsData.glowAmount;
            if (glowAmount > 1f) glowAmount = 1f;

            // Apply lighting effect
            lightAmount = settingsData.lightAmount;
            if (lightAmount < 0.5f) lightAmount = 0.5f;
            else if (lightAmount > 1f) lightAmount = 1f;

            // Get keybinds from file
            try
            {
                Keybinds.move_up = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_up);
                Keybinds.move_left = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_left);
                Keybinds.move_down = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_down);
                Keybinds.move_right = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_move_right);
                Keybinds.dash = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_dash);
                Keybinds.primary = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_primary);
                Keybinds.secondary = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_secondary);
                Keybinds.burn = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_burn);
                Keybinds.escape = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_escape);
                Keybinds.stats = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_stats);
                Keybinds.autofire = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_autofire);
            }
            catch 
            {
                Debug.Log("Ran into issue with keybind! Resetting to default");
                Keybinds.SetDefaultKeybinds(); 
            }

            // Try catch debug seperately as it's a post launch keybind
            try { Keybinds.stats = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_stats); }
            catch { Keybinds.stats = KeyCode.Tab; }

            // Try catch debug seperately as it's a post launch keybind
            try { Keybinds.autofire = (KeyCode)System.Enum.Parse(typeof(KeyCode), settingsData.keybind_autofire); }
            catch { Keybinds.autofire = KeyCode.Space; }

            // Apply settings
            UpdateVideoSettings();
            UpdateVolumeSettings();

            SettingsUI.active.SetGlowAmount(glowAmount);
            SettingsUI.active.SetLightAmount(lightAmount);

            Events.active.UpdateShowHP(alwaysShowHP);
        }
        else
        {
            // Set default settings
            Keybinds.SetDefaultKeybinds();
            music = 1f;
            sound = 1f;
            resolutionX = 1920;
            resolutionY = 1080;
            screenmode = true;
            framerate = 999;
            glowAmount = 0.5f;
            lightAmount = 1f;
            shipColoring = true;
            screenShake = true;
            useParticles = true;
            alwaysShowHP = false;
            damageNumbers = true;
            compoundXP = false;
            musicPitching = true;
            verticalsync = false;
            controllerInput = true;
            xpTrails = true;

            SettingsUI.active.SetGlowAmount(glowAmount);
            SettingsUI.active.SetLightAmount(lightAmount);
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
        // Set frame rate
        if (amount >= 30) framerate = amount;
        UpdateVideoSettings();
    }

    // Set video componenets
    public static void SetVerticalSync(bool toggle)
    {
        // Check if vsync should be applied
        if (toggle) QualitySettings.vSyncCount = 1;
        else QualitySettings.vSyncCount = 0;
        verticalsync = toggle;
    }

    // Apply settings
    public static void UpdateVideoSettings()
    {
        SetVerticalSync(verticalsync);
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
