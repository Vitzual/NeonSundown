using HeathenEngineering.SteamworksIntegration;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Sirenix.OdinInspector;

/// <summary>
/// IMPORTANT NOTE IF YOU'RE GETTING ERRORS IN THIS SCRIPT:
/// - The authenticator script is not available in the GitHub for obvious reasons
/// - Replace any Authenticator.Login() calls with OpenMain()
/// - Remove any Authenticator.UserAuthenticated calls
/// </summary>

public class Menu : MonoBehaviour
{
    // Menu panels
    public ArenaPanel arenaPanel;
    public ShipPanel shipPanel;
    public Store storePanel;
    public ModuleData defaultStoreModule;

    // Group CG elements
    public CanvasGroup mainGroup;
    public CanvasGroup mainBackground;
    public CanvasGroup levelGroup;
    public CanvasGroup achievementGroup;
    public CanvasGroup arenaGroup;
    public CanvasGroup planningGroup;
    public CanvasGroup catalogGroup;
    public CanvasGroup storeGroup;
    public CanvasGroup blackmarketGroup;
    public CanvasGroup crystalsGroup;
    public CanvasGroup buttonsGroup;
    public CanvasGroup socialsGroup;
    public CanvasGroup titleGroup;
    public CanvasGroup pressSpace;
    public CanvasGroup changelogGroup;
    public CanvasGroup creditsGroup;
    public CanvasGroup levelsGroup;
    public CanvasGroup warningGroup;
    public GameObject lockedStoreDesc, normalStoreDesc, offlineMode,
        joinedDiscord, notJoinedDiscord;
    public Image pressSpaceBg;

    // Other interface options
    public RectTransform title;
    public Vector2 titleTargetPos = new Vector2(0, 200);
    public float titleAnimSpeed;
    public Vector2 titleTargetSize = new Vector2(0.7f, 0.7f);
    public float titleSizeSpeed;

    // Press space text element
    public TextMeshProUGUI pressSpaceText, errorText;
    public bool increaseAlpha = false;
    public static bool roadmapWarningShown = false;
    public ArenaData alphaArena;
    public ShipData alphaShip;
    public float alphaAdjustSpeed = 0.1f;
    public AudioClip buttonSound;

    // Menu flags
    private bool spacePressed = false;
    private CanvasGroup currentOpening;
    private float authenticationTimer = 10f;
    private bool mainOpened = false;

    // Start is called before the first frame update
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
        SaveSystem.GetSave();
    }

    // On start, try and get meta context
    public void Start()
    {
        // Subscribe to authentication event
        Events.active.onAuthenticationFinished += StartGameWithCheck;
        Events.active.onAuthenticationFailed += OpenWarningPanel;

        // Reset cursor lock state
        Cursor.lockState = CursorLockMode.None;

        // Update unlocks
        Levels.UpdateUnlocks();

        // Reset effects always
        Effects.ToggleMainGlitchEffect(false);

        // Reset module slots if applicable
        Gamemode.modules = new Dictionary<int, ModuleData>();
        Gamemode.startingCards = new List<CardData>();
        Gamemode.blacklistCards = new List<CardData>();

        // Attempt to get the metacontext on file
        MetaContext context = SaveSystem.GetMetacontext();

        // If not null, apply context
        if (context != null)
        {
            // Attempt to grab last arena
            if (Scriptables.arenasDict.ContainsKey(context.lastArena))
            {
                ArenaData arena = Scriptables.arenasDict[context.lastArena];
                if (arena != null)
                {
                    arenaPanel.SetPanel(arena);
                    Color c = arena.buttonColor;
                    pressSpaceBg.color = new Color(c.r, c.g, c.b, 0.25f);
                }
            }

            // Attempt to grab last ship
            if (Scriptables.shipsDict.ContainsKey(context.lastShip))
            {
                ShipData ship = Scriptables.shipsDict[context.lastShip];
                if (ship != null) shipPanel.Setup(ship);
                else shipPanel.Setup(alphaShip);
            }
            else shipPanel.Setup(alphaShip);

            // Attempt to grab last audio mod
            if (Scriptables.audioModsDict.ContainsKey(context.lastSong))
            {
                AudioData music = Scriptables.audioModsDict[context.lastSong];
                if (music != null) MusicPlayer.SetMusicData(music);
            }

            // Attempt to grab last modules
            if (context.lastModules != null)
            {
                int index = 0;
                ShipPanel.isLoading = true;
                foreach(string id in context.lastModules)
                {
                    if (Scriptables.modulesDict.ContainsKey(id))
                    {
                        ModuleData module = Scriptables.modulesDict[id];
                        if (module != null)
                        {
                            shipPanel.moduleSlot = index;
                            shipPanel.SetModule(module);
                        }
                    }
                    index += 1;
                }
                ShipPanel.isLoading = false;
            }
        }
        else shipPanel.Setup(alphaShip);

        // Check discord reward
        if (SaveSystem.saveData != null)
        {
            notJoinedDiscord.SetActive(!SaveSystem.saveData.discordReward);
            joinedDiscord.SetActive(SaveSystem.saveData.discordReward);
        }
    }

    // Update user input
    public void Update()
    {
        if (!spacePressed && Input.anyKey)
        {
            spacePressed = true;
            if (!Authenticator.UserAuthenticated)
            {
                pressSpaceText.text = "LOGGING INTO STEAM...";
                Authenticator.Login();
            }
            else StartGameWithCheck();
        }
        else if (!mainOpened && spacePressed && !Authenticator.UserAuthenticated)
        {
            if (authenticationTimer <= 0f)
            {
                StartGameWithCheck();
            }
            else authenticationTimer -= Time.deltaTime;
        } 
    }

    // Open main panel
    public void StartGameWithCheck()
    {
        // Open main flag
        mainOpened = true;

        // Remove this if-statement if it's erroring out
        if (!Authenticator.UserAuthenticated)
        {
            OpenWarningPanel("Connection to server timed out");
        }
        else
        {
            lockedStoreDesc.SetActive(false);
            normalStoreDesc.SetActive(true);
            offlineMode.SetActive(false);

            OpenMain();
        }
    }

    public void OpenWarningPanel(string errorMsg)
    {
        // Open main flag
        mainOpened = true;
        TogglePanel(warningGroup, mainGroup, false);
        errorText.text = "<color=orange><b>ERROR:</b></color> " + errorMsg;
    }

    public void StartGameLocked()
    {
        lockedStoreDesc.SetActive(true);
        normalStoreDesc.SetActive(false);
        offlineMode.SetActive(true);

        OpenMain();
    }

    public void OpenMain()
    {
        TogglePanel(mainGroup, warningGroup);
        LeanTween.scale(title, titleTargetSize, titleSizeSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
        LeanTween.moveLocal(title.gameObject, titleTargetPos, titleAnimSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
        LeanTween.alphaCanvas(pressSpace, 0f, titleAnimSpeed);
        LeanTween.alphaCanvas(buttonsGroup, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
        LeanTween.alphaCanvas(socialsGroup, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
        LeanTween.alphaCanvas(mainBackground, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
        LeanTween.alphaCanvas(levelGroup, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
        buttonsGroup.interactable = true;
        buttonsGroup.blocksRaycasts = true;
        socialsGroup.interactable = true;
        socialsGroup.blocksRaycasts = true;
    }

    // Open canvas group panel
    public void TogglePanel(CanvasGroup open, CanvasGroup close, bool playSound = true)
    {
        // Toggle the panels
        LeanTween.cancel(open.gameObject);
        LeanTween.alphaCanvas(open, 1f, 0.35f).setDelay(0.30f);
        open.interactable = true;
        open.blocksRaycasts = true;
        currentOpening = open;
        LeanTween.cancel(close.gameObject);
        LeanTween.alphaCanvas(close, 0f, 0.35f);
        close.interactable = false;
        close.blocksRaycasts = false;
        if (playSound) AudioPlayer.Play(buttonSound, false);
    }

    // Open arena panel
    public void ToggleChangelog(bool toggle)
    {
        if (toggle) TogglePanel(changelogGroup, mainGroup);
        else TogglePanel(mainGroup, changelogGroup);
    }

    // Open arena panel
    public void ToggleCreditsPanel(bool toggle)
    {
        if (toggle) TogglePanel(creditsGroup, mainGroup);
        else TogglePanel(mainGroup, creditsGroup);
    }

    // Open levels panel
    public void ToggleLevelsPanel(bool toggle)
    {
        if (toggle) TogglePanel(levelsGroup, mainGroup);
        else TogglePanel(mainGroup, levelsGroup);
    }

    // Open levels panel
    public void ToggleAchievementsPanel(bool toggle)
    {
        if (toggle) TogglePanel(achievementGroup, mainGroup);
        else TogglePanel(mainGroup, achievementGroup);
    }

    // Open arena panel
    public void ToggleArenaPanel(bool toggle)
    {
        if (Gamemode.arena == null) arenaPanel.SetPanel(alphaArena);
        if (toggle) TogglePanel(arenaGroup, mainGroup);
        else TogglePanel(mainGroup, arenaGroup);
    }

    // Open planning panel
    public void TogglePlanningPanel(bool toggle)
    {
        if (toggle) TogglePanel(planningGroup, arenaGroup);
        else TogglePanel(arenaGroup, planningGroup);
    }

    // Open store panel
    public void ToggleStorePanel(bool toggle)
    {
        if (!Authenticator.UserAuthenticated) return;
        if (toggle) TogglePanel(storeGroup, mainGroup);
        else TogglePanel(mainGroup, storeGroup);
    }

    // Open crystals panel
    public void ToggleCrystalsPanel(bool toggle)
    {
        if (toggle)
        {
            TogglePanel(crystalsGroup, storeGroup);
            storePanel.UpdateCrystals();
            storePanel.SetPanel(defaultStoreModule);
        }
        else TogglePanel(storeGroup, crystalsGroup);
    }

    // Open black market panel
    public void ToggleBlackmarketPanel(bool toggle)
    {
        if (toggle) TogglePanel(blackmarketGroup, storeGroup);
        else TogglePanel(storeGroup, blackmarketGroup);
    }

    // Update menu
    public void FixedUpdate()
    {
        if (increaseAlpha)
        {
            pressSpaceText.alpha += alphaAdjustSpeed;
            if (pressSpaceText.alpha >= 1f) increaseAlpha = false;
        }
        else
        {
            pressSpaceText.alpha -= alphaAdjustSpeed;
            if (pressSpaceText.alpha <= 0f) increaseAlpha = true;
        }
    }

    // Load the main scene
    public void LoadMain()
    {
        // Save game to update modules
        SaveSystem.UpdateSave();

        // Get the equipped modules
        List<string> moduleIDs = new List<string>();
        if (Gamemode.modules != null)
        {
            foreach (KeyValuePair<int, ModuleData> module in Gamemode.modules)
                if (module.Value != null) moduleIDs.Add(module.Value.InternalID);
        }
        else Debug.Log("[ERROR] Gamemode modules were null!");

        // Set meta context and save
        if (Gamemode.arena == null) 
        {
            Debug.Log("[ERROR] Arena is null!");
            return; 
        }
        else if (Gamemode.shipData == null)
        { 
            Debug.Log("[ERROR] Ship is null!");
            return; 
        }
        else SaveSystem.SetMetacontext(Gamemode.arena.InternalID, Gamemode.shipData.InternalID,
            MusicPlayer.GetMusicData().InternalID, moduleIDs);

        // Load the main scene
        SceneManager.LoadScene("Main");
    }

    // Exit the application
    public void QuitGame()
    {
        Authenticator.Logout();
        Application.Quit();
    }

    // Open link
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    public void OpenDiscord(string link)
    {
        if (!SaveSystem.saveData.discordReward)
        {
            SaveSystem.saveData.discordReward = true;
            foreach (CrystalData crystal in Scriptables.crystals)
                SaveSystem.AddCrystal(crystal.InternalID, 5);
            SaveSystem.UpdateSave();

            notJoinedDiscord.SetActive(false);
            joinedDiscord.SetActive(true);
        }
        OpenLink(link);
    }

    // Editor controls - collapse if expanded
    [DisableInPlayMode, Button] private void SetTitlePositionToEnd()
    {
        title.localPosition = titleTargetPos;
        title.localScale = titleTargetSize;
        pressSpace.alpha = 0f;
        buttonsGroup.alpha = 1f;
        socialsGroup.alpha = 1f;
        mainBackground.alpha = 1f;
        levelGroup.alpha = 1f;
        buttonsGroup.interactable = true;
        buttonsGroup.blocksRaycasts = true;
        socialsGroup.interactable = true;
        socialsGroup.blocksRaycasts = true;
        spacePressed = true;
    }
    [DisableInPlayMode, Button] private void SetTitlePositionToStart()
    {
        title.localPosition = Vector3.zero;
        title.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        pressSpace.alpha = 1f;
        buttonsGroup.alpha = 0f;
        socialsGroup.alpha = 0f;
        mainBackground.alpha = 0f;
        levelGroup.alpha = 0f;
        buttonsGroup.interactable = false;
        buttonsGroup.blocksRaycasts = false;
        socialsGroup.interactable = false;
        socialsGroup.blocksRaycasts = false;
        spacePressed = false;
    }
}
