using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StagesPanel : MonoBehaviour
{
    // Internal arena
    [HideInInspector]
    public ArenaData arena;
    
    // List of arenas 
    public List<StageButton> stageButtons;
    [HideInInspector]
    public int stageButtonIndex = 0;

    // Canvas groups
    public CanvasGroup arenaPanel;
    public CanvasGroup stagesPanel;

    // Arena info elements
    public TextMeshProUGUI arenaName, arenaSubtitle, stageTitle, stageTime;
    public Image arenaIcon, stagesBackground, previous, next;

    // Set the panel
    public void Set(ArenaData arena)
    {
        // Set arena data
        this.arena = arena;

        // Set the panel
        arenaName.text = arena.name;
        arenaSubtitle.color = arena.lightColor;
        arenaIcon.sprite = arena.icon;
        stagesBackground.color = arena.buttonColor;
        previous.color = arena.buttonColor;
        next.color = arena.buttonColor;

        // Open the panel
        Toggle(true);
    }

    // Toggle the panel
    public void Toggle(bool toggle)
    {
        if (toggle)
        {
            arenaPanel.alpha = 0f;
            arenaPanel.interactable = false;
            arenaPanel.blocksRaycasts = false;

            stagesPanel.alpha = 1f;
            stagesPanel.interactable = true;
            stagesPanel.blocksRaycasts = true;
        }
        else
        {
            stagesPanel.alpha = 0f;
            stagesPanel.interactable = false;
            stagesPanel.blocksRaycasts = false;

            arenaPanel.alpha = 1f;
            arenaPanel.interactable = true;
            arenaPanel.blocksRaycasts = true;
        }
    }

    // Go to the next stage
    public void SwitchStage(int increase)
    {
        // Move to the next stage by value
        int index = 0; // Holds last button index
        int newStage = stageButtonIndex + increase;
        if (newStage >= 0 && newStage < arena.stages.Count)
        {
            // Set the new stage
            StageData stage = arena.stages[newStage];
            stageTitle.text = stage.name;

            // Calculate time
            stageTime.text = stage.GetTime();
            stageTime.color = arena.lightColor;

            // Iterate through stage enemies
            foreach(StageData.Enemy enemy in stage.enemies)
            {
                // Set stage button to active
                stageButtons[index].gameObject.SetActive(true);
                stageButtons[index].Set(enemy);

                // Increase index 
                index += 1;
            }

            // Increase stage button
            stageButtonIndex += increase;

            // Hide remaining buttons
            while (index < stageButtons.Count)
            {
                stageButtons[index].gameObject.SetActive(false);
                index += 1;
            }
        }
    }
}
