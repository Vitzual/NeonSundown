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
        foreach (SynergyData synergy in Scriptables.synergies)
        {
            if (synergy.IsMasterSynergy())
            {
                SynergyElement newSynergy = Instantiate(synergyElement, Vector2.zero, Quaternion.identity);
                newSynergy.transform.SetParent(list);
                newSynergy.transform.localScale = Vector3.one;
                newSynergy.Set(synergy);
                synergies.Add(newSynergy);
            }
        }

        // Organize synergies (going to update this later)
        foreach (SynergyElement element in synergies)
            element.transform.SetSiblingIndex(element.synergyData.order);
        foreach (SynergyElement element in synergies)
            element.transform.SetSiblingIndex(element.synergyData.order);
        foreach (SynergyElement element in synergies)
            element.transform.SetSiblingIndex(element.synergyData.order);
        foreach (SynergyElement element in synergies)
            element.transform.SetSiblingIndex(element.synergyData.order);
        foreach (SynergyElement element in synergies)
            element.transform.SetSiblingIndex(element.synergyData.order);
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
