using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    // List of keybinds
    [System.Serializable]
    public class Keybind
    {
        public Key key;
        public ButtonManagerBasic button;
    }

    public List<Keybind> keybinds;
    private Key listeningForKey;
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
            // Grab input and capitalize it if needed
            string input = Input.inputString.ToUpper();
            if (input == "")
            {
                // Debug to player thing
                Debug.Log("Invalid key press detected, checking specific cases...");

                // Get other supported inputs
                input = CheckOtherInputs();

                // Check if input is now valid
                if (input == "")
                {
                    Debug.Log("Input is not supported! Please contact me (Ben) to get support for this device / input key.");
                    return;
                }
                else Debug.Log("Matched invalid key press to " + input);
            }
            else if (input == " ") input = "Space";

            // Debug to player thing
            Debug.Log("Attempting to change " + listeningForKey + " to " + input);
            Keybinds.SetKeybind(listeningForKey, (KeyCode)System.Enum.Parse(typeof(KeyCode), input));
            listeningForInput = false;

            // Loop through keybinds and change key
            foreach (Keybind keybind in keybinds)
            {
                if (keybind.key == listeningForKey)
                {
                    keybind.button.buttonText = input;
                    keybind.button.UpdateUI();
                }
            }
        }
    }

    // Set keybinds methods
    public void SetKeybind(int number)
    {
        Key key = (Key)number;

        listeningForInput = true;
        listeningForKey = key;

        foreach (Keybind keybind in keybinds)
        {
            if (keybind.key == key)
            {
                keybind.button.buttonText = "Press any key...";
                keybind.button.UpdateUI();
            }
        }
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
        foreach (Keybind keybind in keybinds)
        {
            keybind.button.buttonText = Keybinds.GetKeybind(keybind.key).ToString();
            keybind.button.UpdateUI();
        }
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

    public string CheckOtherInputs()
    {
        // Check mouse input
        if (Input.GetMouseButtonDown(0)) return "Mouse0";
        else if (Input.GetMouseButtonDown(1)) return "Mouse1";
        else if (Input.GetMouseButtonDown(2)) return "Mouse2";
        else if (Input.GetMouseButtonDown(3)) return "Mouse3";
        else if (Input.GetMouseButtonDown(4)) return "Mouse4";
        else if (Input.GetMouseButtonDown(5)) return "Mouse5";
        else if (Input.GetKeyDown(KeyCode.Escape)) return "Escape";
        else if (Input.GetKeyDown(KeyCode.Tab)) return "Tab";
        else if (Input.GetKeyDown(KeyCode.CapsLock)) return "CapsLock";
        else if (Input.GetKeyDown(KeyCode.LeftShift)) return "LeftShift";
        else if (Input.GetKeyDown(KeyCode.Space)) return "Space";
        else if (Input.GetKeyDown(KeyCode.LeftControl)) return "LeftControl";
        else if (Input.GetKeyDown(KeyCode.UpArrow)) return "UpArrow";
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) return "LeftArrow";
        else if (Input.GetKeyDown(KeyCode.DownArrow)) return "DownArrow";
        else if (Input.GetKeyDown(KeyCode.RightArrow)) return "RightArrow";
        else return "";
    }
}
