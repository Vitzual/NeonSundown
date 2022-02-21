using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Menu panels
    public ArenaPanel arenaPanel;
    public PlanningPanel planningPanel;
    public Store storePanel;
    public ModuleData defaultStoreModule;

    // Arena elements
    public ArenaButton arenaButton;
    public Transform arenaList;

    // Group CG elements
    public CanvasGroup mainGroup;
    public CanvasGroup mainBackground;
    public CanvasGroup arenaGroup;
    public CanvasGroup planningGroup;
    public CanvasGroup catalogGroup;
    public CanvasGroup storeGroup;
    public CanvasGroup buttonsGroup;
    public CanvasGroup socialsGroup;
    public CanvasGroup titleGroup;
    public CanvasGroup pressSpace;

    // Other interface options
    public RectTransform title;
    public Vector2 titleTargetPos = new Vector2(0, 200);
    public float titleAnimSpeed;
    public Vector2 titleTargetSize = new Vector2(0.7f, 0.7f);
    public float titleSizeSpeed;

    // Press space text element
    public TextMeshProUGUI pressSpaceText;
    public bool increaseAlpha = false;
    public float alphaAdjustSpeed = 0.1f;

    // Menu flags
    private bool spacePressed = false;
    private bool arenasGenerated = false;

    // Start is called before the first frame update
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
        SaveSystem.GetSave();
        Settings.LoadSettings();
    }

    // Update user input
    public void Update()
    {
        if (!spacePressed && Input.anyKey)
        {
            LeanTween.scale(title, titleTargetSize, titleSizeSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
            LeanTween.moveLocal(title.gameObject, titleTargetPos, titleAnimSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
            LeanTween.alphaCanvas(pressSpace, 0f, titleAnimSpeed);
            LeanTween.alphaCanvas(buttonsGroup, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            LeanTween.alphaCanvas(socialsGroup, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            LeanTween.alphaCanvas(mainBackground, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            buttonsGroup.interactable = true;
            buttonsGroup.blocksRaycasts = true;
            socialsGroup.interactable = true;
            socialsGroup.blocksRaycasts = true;
            spacePressed = true;
        }
    }

    // Open arena panel
    public void OpenArenas()
    {
        // Check if arenas generated
        if (!arenasGenerated)
        {
            // Get save data
            List<ArenaButton> buttonList = new List<ArenaButton>();

            // Get all arenas on record
            foreach (ArenaData arena in Scriptables.arenas)
            {
                // Create arena buttons
                ArenaButton newButton = Instantiate(arenaButton, Vector2.zero, Quaternion.identity).GetComponent<ArenaButton>();
                newButton.transform.SetParent(arenaList);
                buttonList.Add(newButton);

                // Check if save is unlocked, and if so set time
                if (SaveSystem.IsArenaUnlocked(arena))
                {
                    if (SaveSystem.saveData.arenas.ContainsKey(arena.InternalID))
                    {
                        newButton.Set(arena, "<b>Best Run:</b> " + Formatter.Time
                            (SaveSystem.saveData.arenas[arena.InternalID].bestTime));
                    }
                    else newButton.Set(arena, "<b>Best Run:</b> 0:00");
                }

                // If arena not unlocked, show it as locked
                else newButton.Lock(arena);
            }

            // Iterate through all generated buttons and set order
            foreach(ArenaButton button in buttonList)
                button.transform.SetSiblingIndex(button.arena.order);

            // Set arenas generated flag to true
            arenasGenerated = true;
        }

        // Change menu
        ToggleArenaPanel(true);
    }

    // Open canvas group panel
    public void TogglePanel(CanvasGroup open, CanvasGroup close)
    {
        LeanTween.alphaCanvas(open, 1f, 0.35f).setDelay(0.30f);
        open.interactable = true;
        open.blocksRaycasts = true;
        LeanTween.alphaCanvas(close, 0f, 0.35f);
        close.interactable = false;
        close.blocksRaycasts = false;
    }

    // Open arena panel
    public void ToggleArenaPanel(bool toggle)
    {
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