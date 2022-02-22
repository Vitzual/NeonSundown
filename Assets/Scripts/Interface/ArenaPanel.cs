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
    }

    // Background tilebase
    public Tilemap backgroundTilemap;

    // Canvas group
    public Image panelBackground;
    public CanvasGroup panelGroup;
    public CanvasGroup selectArena;
    public CanvasGroup arenaLocked;
    public Color emptyCardColor;

    // Panel variables
    public ProgressBar progressBar;
    public Image barBackground, barFill;
    public TextMeshProUGUI arena, description, timestampOne, timestampTwo, timestampThree,
        timestampFour, rewardOne, rewardTwo, rewardThree, rewardFour, bestTime;
    public Image rewardOneImage, rewardTwoImage, rewardThreeImage, rewardFourImage;
    public List<Card> startingCards;
    public List<Card> blacklistCards;

    // Subscribe to the arena button event
    public void Start()
    {
        Events.active.onArenaButtonClicked += SetPanel;
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

        // Set arena information
        this.arena.text = arena.name;
        description.text = arena.desc;

        // Set best time
        float time = SaveSystem.GetBestTime(arena.InternalID);
        float maxTime = arena.objectiveFour.timeRequired;
        progressBar.maxValue = maxTime;
        progressBar.currentPercent = (time / maxTime) * 100f;
        progressBar.UpdateUI();
        bestTime.text = "BEST TIME:<br><size=60>" + Formatter.Time(time);

        // Set arena objective one
        timestampOne.text = Formatter.Time(arena.objectiveOne.timeRequired);
        rewardOne.text = arena.objectiveOne.rewardName;
        rewardOneImage.sprite = arena.objectiveOne.rewardImage;

        // Set arena objective two
        timestampTwo.text = Formatter.Time(arena.objectiveTwo.timeRequired);
        rewardTwo.text = arena.objectiveTwo.rewardName;
        rewardTwoImage.sprite = arena.objectiveTwo.rewardImage;

        // Set arena objective three
        timestampThree.text = Formatter.Time(arena.objectiveThree.timeRequired);
        rewardThree.text = arena.objectiveThree.rewardName;
        rewardThreeImage.sprite = arena.objectiveThree.rewardImage;

        // Set arena objective four
        timestampFour.text = Formatter.Time(arena.objectiveFour.timeRequired);
        rewardFour.text = arena.objectiveFour.rewardName;
        rewardFourImage.sprite = arena.objectiveFour.rewardImage;

        // Set starting cards information
        for (int i = 0; i < startingCards.Count; i++)
        {
            if (i < arena.startingCards.Count)
            {
                startingCards[i].icon.gameObject.SetActive(true);
                startingCards[i].icon.sprite = arena.startingCards[i].sprite;
                startingCards[i].icon.color = arena.startingCards[i].color;
                startingCards[i].model.color = arena.startingCards[i].color;
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

        // Change the background
        ChangeBackground(arena);
    }

    public void ResetPanel()
    {
        // Set starting cards information
        for (int i = 0; i < startingCards.Count; i++)
        {
            startingCards[i].icon.gameObject.SetActive(false);
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
