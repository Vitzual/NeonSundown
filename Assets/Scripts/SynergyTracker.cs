using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyTracker : MonoBehaviour
{
    // Synergy progress
    public static bool isActive = false;
    public SynergyElement synergyElement;
    public List<SynergyElement> synergies;
    public CanvasGroup canvasGroup;
    public Transform list;

    // On start create synergies
    public void Start()
    {
        // Create all the synergies
        foreach(LevelData level in Levels.ranks)
        {
            if (level.synergyReward != null && !level.synergyReward.IsTierOne())
            {
                SynergyElement newSynergy = Instantiate(synergyElement, Vector2.zero, Quaternion.identity);
                newSynergy.transform.SetParent(list);
                newSynergy.transform.localScale = Vector3.one;
                newSynergy.Set(level.synergyReward);
                synergies.Add(newSynergy);
            }
        }
    }

    // Update the synergies
    public void UpdateSynergies()
    {
        foreach (SynergyElement synergy in synergies)
            synergy.UpdateSynergies();
    }

    // Open and close my guy
    public void Toggle(bool toggle)
    {
        if (toggle)
        {
            isActive = true;
            UpdateSynergies();
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            PausedMenu.canvasGroup.alpha = 0f;
            PausedMenu.canvasGroup.interactable = false;
            PausedMenu.canvasGroup.blocksRaycasts = false;
        }
        else
        {
            isActive = false;
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            PausedMenu.canvasGroup.alpha = 1f;
            PausedMenu.canvasGroup.interactable = true;
            PausedMenu.canvasGroup.blocksRaycasts = true;
        }
    }
}
