using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ArenaPanel : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public Image icon;
        public Image model;
        public TextMeshProUGUI amount;
    }

    // Stages panel
    public StagesPanel stagesPanel;

    // Arena elements
    public ArenaButton arenaButton;
    public Transform arenaList;

    // Background tilebase
    public Tilemap backgroundTilemap;
    public List<Card> blacklistCards;
    public List<Card> startingCards;

    // Canvas group
    public Image panelBackground;
    public CanvasGroup panelGroup;
    public CanvasGroup selectArena;
    public CanvasGroup arenaLocked;
    public Color emptyCardColor;

    // Panel variables
    public ProgressBar progressBar;
    public Image barBackground, barFill, titleBarLeft, titleBarRight, arenaViewRules;
    public TextMeshProUGUI arena, description, timestampOne, timestampTwo, timestampThree,
        timestampFour, rewardOne, rewardTwo, rewardThree, rewardFour, bestTime;
    public Image rewardOneImage, rewardTwoImage, rewardThreeImage, rewardFourImage,
        lineOne, lineTwo, lineThree, lineFour;

    // Private internal flag
    private bool arenasGenerated = false;

    // Subscribe to the arena button event
    public void Start()
    {
        // Setup events
        Events.active.onArenaButtonClicked += SetPanel;

        // Check if arenas already generated
        if (!arenasGenerated)
        {
            // Setup arenas
            List<ArenaButton> buttonList = new List<ArenaButton>();

            // Iterate through all arenas
            foreach (ArenaData arena in Scriptables.arenas)
            {
                // Create arena buttons
                ArenaButton newButton = Instantiate(arenaButton, Vector2.zero, Quaternion.identity);
                newButton.transform.SetParent(arenaList);
                RectTransform rect = newButton.GetComponent<RectTransform>();
                rect.localScale = new Vector3(1, 1, 1);
                buttonList.Add(newButton);

                // Check if save is unlocked, and if so set time
                if (arena.unlockByDefault || SaveSystem.IsArenaUnlocked(arena.InternalID))
                {
                    if (SaveSystem.saveData.arenaTimes.ContainsKey(arena.InternalID))
                    {
                        newButton.Set(arena, "<b>Best Run:</b> " + Formatter.Time
                            (SaveSystem.saveData.arenaTimes[arena.InternalID]));
                    }
                    else newButton.Set(arena, "<b>Best Run:</b> 0:00");
                }

                // If arena not unlocked, show it as locked
                else newButton.Lock(arena);
            }

            // I'll come back to this later. Unity is being a dummy and
            // not giving two shits about setting the sibling index

            // Iterate through all generated buttons and set order
            foreach (ArenaButton button in buttonList)
                button.gameObject.transform.SetSiblingIndex(button.arena.order);

            // Iterate through all generated buttons and set order
            foreach (ArenaButton button in buttonList)
                button.gameObject.transform.SetSiblingIndex(button.arena.order);

            // Iterate through all generated buttons and set order
            foreach (ArenaButton button in buttonList)
                button.gameObject.transform.SetSiblingIndex(button.arena.order);

            // Set arenas generated flag to true
            arenasGenerated = true;
        }
    }

    public void SetPanel(ArenaData arena)
    {
        // Set the new arena data
        Gamemode.arena = arena;

        // Reset the panel
        ResetPanel();
        panelGroup.alpha = 1f;
        selectArena.alpha = 0f;
        panelBackground.color = arena.buttonColor;
        barBackground.color = new Color(arena.buttonColor.r, arena.buttonColor.g, arena.buttonColor.b, 0.1f);
        barFill.color = arena.buttonColor;
        titleBarLeft.color = arena.buttonColor;
        titleBarRight.color = arena.buttonColor;
        arenaViewRules.color = arena.buttonColor;

        // Set arena information
        this.arena.text = arena.name;
        description.text = arena.desc;

        // Set best time
        float time = SaveSystem.GetBestTime(arena.InternalID);
        float maxTime = arena.objectiveFour.timeRequired;
        progressBar.maxValue = maxTime;
        progressBar.currentPercent = time;
        progressBar.UpdateUI();
        bestTime.text = "BEST TIME:<br><size=60>" + Formatter.Time(time);

        // Set arena objective one
        timestampOne.text = Formatter.Time(arena.objectiveOne.timeRequired);
        rewardOne.text = arena.objectiveOne.rewardName;
        rewardOneImage.sprite = arena.objectiveOne.rewardImage;
        rewardOneImage.color = arena.objectiveOne.rewardColor;
        lineOne.color = arena.lightColor;

        // Set arena objective two
        timestampTwo.text = Formatter.Time(arena.objectiveTwo.timeRequired);
        rewardTwo.text = arena.objectiveTwo.rewardName;
        rewardTwoImage.sprite = arena.objectiveTwo.rewardImage;
        rewardTwoImage.color = arena.objectiveTwo.rewardColor;
        lineTwo.color = arena.lightColor;

        // Set arena objective three
        timestampThree.text = Formatter.Time(arena.objectiveThree.timeRequired);
        rewardThree.text = arena.objectiveThree.rewardName;
        rewardThreeImage.sprite = arena.objectiveThree.rewardImage;
        rewardThreeImage.color = arena.objectiveThree.rewardColor;
        lineThree.color = arena.lightColor;

        // Set arena objective four
        timestampFour.text = Formatter.Time(arena.objectiveFour.timeRequired);
        rewardFour.text = arena.objectiveFour.rewardName;
        rewardFourImage.sprite = arena.objectiveFour.rewardImage;
        rewardFourImage.color = arena.objectiveFour.rewardColor;
        lineFour.color = arena.lightColor;

        // Set starting cards information
        for (int i = 0; i < startingCards.Count; i++)
        {
            if (i < arena.startingCards.Count)
            {
                startingCards[i].icon.gameObject.SetActive(true);
                startingCards[i].amount.gameObject.SetActive(true);
                startingCards[i].icon.sprite = arena.startingCards[i].card.sprite;
                startingCards[i].icon.color = arena.startingCards[i].card.color;
                startingCards[i].model.color = arena.startingCards[i].card.color;
                startingCards[i].amount.text = "LEVEL " + arena.startingCards[i].amount;
            }
            else break;
        }

        // Set starting cards information
        for (int i = 0; i < blacklistCards.Count; i++)
        {
            if (i < arena.blacklistCards.Count)
            {
                blacklistCards[i].icon.gameObject.SetActive(true);
                blacklistCards[i].icon.sprite = arena.blacklistCards[i].sprite;
                blacklistCards[i].icon.color = arena.blacklistCards[i].color;
                blacklistCards[i].model.color = arena.blacklistCards[i].color;
            }
            else break;
        }

        // Set menu music
        if (MusicPlayer.isMenu)
        {
            Debug.Log("Setting music pitch to " + arena.arenaMenuPitch);
            MusicPlayer.music.pitch = arena.arenaMenuPitch;
        }

        // Change the background
        ChangeBackground(arena);
    }

    public void ViewArena()
    {
        stagesPanel.Set(Gamemode.arena);
        stagesPanel.stageButtonIndex = 0;
        stagesPanel.SwitchStage(0);
    }

    public void ResetPanel()
    {
        // Set starting cards information
        for (int i = 0; i < startingCards.Count; i++)
        {
            startingCards[i].icon.gameObject.SetActive(false);
            startingCards[i].amount.gameObject.SetActive(false);
            startingCards[i].model.color = emptyCardColor;
        }

        // Set starting cards information
        for (int i = 0; i < blacklistCards.Count; i++)
        {
            blacklistCards[i].icon.gameObject.SetActive(false);
            blacklistCards[i].model.color = emptyCardColor;
        }
    }

    public void ChangeBackground(ArenaData arena)
    {
        // Check if stage already active
        if (MenuSpawner.active.menuStage == arena.menuStage) return;

        // Set the arena stage
        MenuSpawner.active.menuStage = arena.menuStage;

        // Wipe the old background
        MenuSpawner.active.WipeEnemies();

        // Set the background
        for (int x = -4; x < 4; x++)
            for (int y = -4; y < 4; y++)
                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), arena.arenaBackground);
     }
}
