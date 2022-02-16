using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    // List of keybinds
    public class Keybind
    {
        public Keybinds.Key key;
        public ButtonManagerBasic button;
    }
    public List<Keybind> keybinds;
    private Keybinds.Key listeningForKey;
    private bool listeningForInput = false;

    // Active
    public static SettingsUI active;
    public static bool isOpen;

    // Variables
    public SliderManager music;
    public SliderManager sound;

    // Get stuff
    public GameObject pausedMenu;

    // Canvas group
    public CanvasGroup canvasGroup;

    // Get active instance
    public void Awake()
    {
        active = this;
        canvasGroup = GetComponent<CanvasGroup>();
        Settings.LoadSettings();
    }

    // Get input from player
    public void Update()
    {
        if (listeningForInput && Input.anyKeyDown)
        {
            Keybinds.SetKeybind(listeningForKey, (KeyCode)System.Enum.Parse(typeof(KeyCode), Input.inputString));
            listeningForInput = false;

            foreach (Keybind keybind in keybinds)
                if (keybind.key == listeningForKey)
                    keybind.button.buttonText = Input.inputString;
        }
    }

    // Set keybinds methods
    public void SetKeybind(Keybinds.Key key)
    {
        listeningForInput = true;
        listeningForKey = key;

        foreach (Keybind keybind in keybinds)
            if (keybind.key == key)
                keybind.button.buttonText = "Press any key...";
    }

    // Set volume control methods
    public void SetMaster(float value) { Settings.SetVolume(Settings.Volume.Master, value); }
    public void SetMusic(float value) { Settings.SetVolume(Settings.Volume.Music, value); }
    public void SetSound(float value) { Settings.SetVolume(Settings.Volume.Sound, value); }

    // Set video settings methods
    public void SwitchScreenmode(bool fullscreen) { Settings.SetScreenMode(fullscreen); }
    public void SwitchFramerate(int amount) { Settings.SetFramerate(amount); }

    // Switch resolution
    public void SwitchResolution(int option)
    {
        switch(option)
        {
            case 0:
                Settings.SetResolution(640, 480);
                break;
            case 1:
                Settings.SetResolution(1280, 720);
                break;
            case 2:
                Settings.SetResolution(1366, 768);
                break;
            case 3:
                Settings.SetResolution(1440, 900);
                break;
            case 4:
                Settings.SetResolution(1920, 1080);
                break;
            case 5:
                Settings.SetResolution(2560, 1080);
                break;
            case 6:
                Settings.SetResolution(2560, 1440);
                break;
            case 7:
                Settings.SetResolution(3440, 1440);
                break;
            case 8:
                Settings.SetResolution(3840, 2160);
                break;
        }
        Settings.UpdateVideoSettings();
    }

    // Go back
    public void Enable()
    {
        // Set values
        music.mainSlider.value = Settings.music;
        sound.mainSlider.value = Settings.sound;

        // Update values
        music.UpdateUI();
        sound.UpdateUI();

        // Update canvas group
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Set to open
        isOpen = true;

        // Close paused menu if it exists
        if (pausedMenu != null)
            pausedMenu.SetActive(false);

        // Set all keybind buttons
        foreach(Keybind keybind in keybinds)
            keybind.button.buttonText = Keybinds.GetKeybind(keybind.key).ToString();
    }

    // Go back
    public void Back()
    {   
        // Update canvas group
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Set to open
        isOpen = false;
        Settings.SaveSettings();

        // Open paused menu if it exists
        if (pausedMenu != null)
            pausedMenu.SetActive(true);
    }
}
