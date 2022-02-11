using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Arena elements
    public ArenaButton arenaButton;
    public Transform arenaList;

    // Group CG elements
    public CanvasGroup mainMenuGroup;
    public CanvasGroup arenaSelectGroup;

    // Other CG elements
    public CanvasGroup menuButtons;
    public CanvasGroup pressSpace;
    public CanvasGroup socialsLeft;
    public CanvasGroup socialsRight;
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
    }

    // Update user input
    public void Update()
    {
        if (!spacePressed && Input.anyKey)
        {
            LeanTween.scale(title, titleTargetSize, titleSizeSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
            LeanTween.moveLocal(title.gameObject, titleTargetPos, titleAnimSpeed).setEase(LeanTweenType.easeInExpo).setDelay(0.2f);
            LeanTween.alphaCanvas(pressSpace, 0f, titleAnimSpeed);
            LeanTween.alphaCanvas(menuButtons, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            LeanTween.alphaCanvas(socialsLeft, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            LeanTween.alphaCanvas(socialsRight, 1f, titleAnimSpeed).setDelay(titleAnimSpeed);
            menuButtons.interactable = true;
            menuButtons.blocksRaycasts = true;
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
            PlayerSave arenaSaves = Loader.GetPlayerSave();
            bool arenasOnRecord = arenaSaves != null;
            List<ArenaButton> buttonList = new List<ArenaButton>();

            // Get all arenas on record
            foreach (ArenaData arena in Scriptables.arenas)
            {
                // Create arena buttons
                ArenaButton newButton = Instantiate(arenaButton, Vector2.zero, Quaternion.identity).GetComponent<ArenaButton>();
                newButton.transform.SetParent(arenaList);
                buttonList.Add(newButton);
                if (arenasOnRecord && arenaSaves.arenas.ContainsKey(arena.InternalID))
                    newButton.Set(arena, "<b>Best Run:</b> " + Formatter.Time(arenaSaves.arenas[arena.InternalID]));
                else newButton.Set(arena, "<b>Best Run:</b> 0:00");
            }

            // Iterate through all generated buttons and set order
            foreach(ArenaButton button in buttonList)
                button.transform.SetSiblingIndex(button.arena.order);

            // Set arenas generated flag to true
            arenasGenerated = true;
        }

        // Change menu
        LeanTween.alphaCanvas(mainMenuGroup, 0f, 0.5f);
        mainMenuGroup.interactable = false;
        mainMenuGroup.blocksRaycasts = false;
        LeanTween.alphaCanvas(arenaSelectGroup, 1f, 0.5f).setDelay(0.25f);
        arenaSelectGroup.interactable = true;
        arenaSelectGroup.blocksRaycasts = true;
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
}
