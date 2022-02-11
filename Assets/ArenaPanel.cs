using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArenaPanel : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public Image icon;
        public Image model;
    }

    // Panel variables
    public TextMeshProUGUI arena, description, difficulty, 
        length, phases, primaryObjective, secondaryObjective;
    public List<Card> startingCards;
    public List<Card> blacklistCards;
    public Color emptyCardColor;

    // Canvas group
    public CanvasGroup panelGroup;

    // Subscribe to the arena button event
    public void Start()
    {
        Events.active.onArenaButtonClicked += SetPanel;
    }

    public void SetPanel(ArenaData arena)
    {
        // Reset the panel
        ResetPanel();
        panelGroup.alpha = 1f;

        // Set arena information
        this.arena.text = arena.name;
        description.text = arena.desc;
        difficulty.text = "<b>DIFFICULTY:</b> " + arena.difficulty;
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
            if (arena.startingCards.Count < i)
            {
                startingCards[i].icon.gameObject.SetActive(true);
                startingCards[i].icon.sprite = arena.startingCards[i].sprite;
                startingCards[i].model.color = arena.startingCards[i].color;
            }
            else break;
        }

        // Set starting cards information
        for (int i = 0; i < blacklistCards.Count; i++)
        {
            if (arena.blacklistCards.Count < i)
            {
                blacklistCards[i].icon.gameObject.SetActive(true);
                blacklistCards[i].icon.sprite = arena.blacklistCards[i].sprite;
                blacklistCards[i].model.color = arena.blacklistCards[i].color;
            }
            else break;
        }
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
}
