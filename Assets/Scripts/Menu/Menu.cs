using HeathenEngineering.SteamworksIntegration;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public CanvasGroup arenaGroup;
    public CanvasGroup planningGroup;
    public CanvasGroup catalogGroup;
    public CanvasGroup storeGroup;
    public CanvasGroup buttonsGroup;
    public CanvasGroup socialsGroup;
    public CanvasGroup titleGroup;
    public CanvasGroup pressSpace;
    public CanvasGroup alphaGroup;
    public CanvasGroup roadmapGroup;
    public CanvasGroup creditsGroup;
    public CanvasGroup roadmapWarningGroup;
    public CanvasGroup levelsGroup;
    public Image pressSpaceBg;

    // Other interface options
    public RectTransform title;
    public Vector2 titleTargetPos = new Vector2(0, 200);
    public float titleAnimSpeed;
    public Vector2 titleTargetSize = new Vector2(0.7f, 0.7f);
    public float titleSizeSpeed;

    // Press space text element
    public TextMeshProUGUI pressSpaceText;
    public bool increaseAlpha = false;
    public bool alphaBuild = false;
    public static bool roadmapWarningShown = false;
    public ArenaData alphaArena;
    public ShipData alphaShip;
    public float alphaAdjustSpeed = 0.1f;
    public AudioClip buttonSound;

    // Menu flags
    private bool spacePressed = false;
    private CanvasGroup currentOpening;

    // Start is called before the first frame update
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
        if (!alphaBuild) SaveSystem.GetSave();
    }

    // On start, try and get meta context
    public void Start()
    {
        // Reset module slots if applicable
        Gamemode.modules = new Dictionary<int, ModuleData>();

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
        }
        else shipPanel.Setup(alphaShip);
    }
    
    // Update user input
    public void Update()
    {
        if (!spacePressed && Input.anyKey)
        {
            if (alphaBuild)
            {
                LeanTween.alphaCanvas(alphaGroup, 1f, titleAnimSpeed);
                alphaGroup.interactable = true;
                alphaGroup.blocksRaycasts = true;
                Gamemode.arena = alphaArena;
                Gamemode.ship = alphaShip;
            }
            else OpenMain();
        }
    }

    // Open main panel
    public void OpenMain()
    {
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
        spacePressed = true;
    }

    // Open canvas group panel
    public void TogglePanel(CanvasGroup open, CanvasGroup close)
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
        AudioPlayer.Play(buttonSound, false);
    }

    // Open arena panel
    public void ToggleRoadmapPanel(bool toggle)
    {
        if (!roadmapWarningShown)
        {
            roadmapWarningShown = true;
            TogglePanel(roadmapWarningGroup, mainGroup);
        }
        else
        {
            DisableRoadmapWarning();
            if (toggle) TogglePanel(roadmapGroup, mainGroup);
            else TogglePanel(mainGroup, roadmapGroup);
        }
    }

    // Disable roadmap panel
    public void DisableRoadmapWarning()
    {
        LeanTween.alphaCanvas(roadmapWarningGroup, 0f, 0.35f);
        roadmapWarningGroup.interactable = false;
        roadmapWarningGroup.blocksRaycasts = false;
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
        if (toggle)
        {
            TogglePanel(storeGroup, mainGroup);
            storePanel.UpdateCrystals();
            storePanel.SetPanel(defaultStoreModule);
        }
        else TogglePanel(mainGroup, storeGroup);
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

        // Set context and load the scene
        SaveSystem.SetMetacontext(Gamemode.arena.InternalID, Gamemode.ship.InternalID, null);
        SceneManager.LoadScene("Main");
    }

    // Exit the application
    public void QuitGame()
    {
        Application.Quit();
    }

    // Open link
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}
