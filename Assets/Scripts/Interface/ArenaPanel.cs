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
    public TextMeshProUGUI arena, description, difficulty,
        length, phases, primaryObjective, secondaryObjective,
        primaryStatus, secondaryStatus;
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

        // Set arena information
        this.arena.text = arena.name;
        description.text = arena.desc;
        difficulty.text = "<b>DIFFICULTY:</b> " + arena.difficulty;
        difficulty.color = arena.difficultyColor;
        length.text = "<b>LENGTH:</b> " + arena.length;
        phases.text = "<b>PHASES:</b> " + arena.stages.Count + " phases";

        // Set arena objectives
        primaryObjective.text = arena.primaryObjective.name + "<br>" +
            "<b>REWARD:</b> " + arena.primaryObjective.reward;
        secondaryObjective.text = arena.secondaryObjective.name + "<br>" +
            "<b>REWARD:</b> " + arena.secondaryObjective.reward;

        // Set starting cards information
        for(int i = 0; i < startingCards.Count; i++)
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
