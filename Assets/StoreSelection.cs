using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreSelection : MonoBehaviour
{
    // UI References
    public Menu menu;
    public BlackmarketPanel panel;

    // Store selection UI elements
    public GameObject blackmarketLocked, blackmarketProgress;
    public ProgressBar moduleBar, blackmarketBar;
    public TextMeshProUGUI moduleAmount, blackmarketAmount;

    public void UpdateButtons()
    {
        // Unlock market panel if over 50
        if (SaveSystem.GetPlayerLevel() >= 50)
        {
            blackmarketLocked.SetActive(false);
            blackmarketProgress.SetActive(true);
        }
        else
        {
            blackmarketLocked.SetActive(true);
            blackmarketProgress.SetActive(false);
        }

        // Update module progress
        int amountCompleted = 0, amountTodo = 0;
        foreach (ModuleData module in Scriptables.modules)
        {
            amountTodo += module.values.Count;
            amountCompleted += SaveSystem.GetModuleAmount(module.InternalID) + 1;
        }
        moduleBar.maxValue = amountTodo;
        moduleBar.currentPercent = amountCompleted;
        moduleBar.UpdateUI();
        moduleAmount.text = amountCompleted + " / " + amountTodo + " COMPLETE";

        // Update blackmarket progress
        int amount = SaveSystem.GetBlackmarketItemsUnlocked();
        if (amount > panel.GetTotalAmountOfItems()) amount = panel.GetTotalAmountOfItems();
        blackmarketAmount.text = amount + " / "  + panel.GetTotalAmountOfItems() + " UNLOCKED";
        blackmarketBar.maxValue = panel.GetTotalAmountOfItems();
        blackmarketBar.currentPercent = amount;
        blackmarketBar.UpdateUI();
    }
    
    public void ToggleStore(bool toggle)
    {
        UpdateButtons();
        menu.ToggleStorePanel(toggle);
    }

    public void ToggleModules(bool toggle)
    {
        if (!toggle) UpdateButtons();
        menu.ToggleCrystalsPanel(toggle);
    }

    public void ToggleBlackmarket(bool toggle)
    {
        if (!toggle) 
        {
            UpdateButtons();
            panel.CancelPreview();
            menu.ToggleBlackmarketPanel(toggle);
        } 
        else if (SaveSystem.GetPlayerLevel() >= 50)
        {
            panel.UpdateResources();
            menu.ToggleBlackmarketPanel(toggle);
        }
    }
}
