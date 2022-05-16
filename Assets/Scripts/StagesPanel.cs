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

    // Arena info elements
    public TextMeshProUGUI stageTitle, stageTime;
    public Image previous, next;

    // Set the panel
    public void Set(ArenaData arena)
    {
        // Set arena data
        this.arena = arena;

        // Set the panel
        previous.color = arena.buttonColor;
        next.color = arena.buttonColor;
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
