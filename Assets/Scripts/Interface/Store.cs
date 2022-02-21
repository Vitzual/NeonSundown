using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    // Interface variables
    public Image moduleIcon;
    public Image moduleBorderOne;
    public Image moduleBorderTwo;
    public Image panelBorder;
    public Image panelButton;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI owned;

    // Crystal variables
    public CrystalData blueCrystal;
    public TextMeshProUGUI blueCrystalAmount;
    public CrystalData greenCrystal;
    public TextMeshProUGUI greenCrystalAmount;
    public CrystalData redCrystal;
    public TextMeshProUGUI redCrystalAmount;

    // Internal variables
    private ModuleData module;

    // Update the crystal amounts
    public void UpdateCrystals()
    {
        // Update blue crystals
        if (SaveSystem.saveData.crystals.ContainsKey(blueCrystal.InternalID))
            blueCrystalAmount.text = SaveSystem.saveData.crystals[blueCrystal.InternalID].ToString();
        else blueCrystalAmount.text = "0";

        // Update green crystals
        if (SaveSystem.saveData.crystals.ContainsKey(greenCrystal.InternalID))
            greenCrystalAmount.text = SaveSystem.saveData.crystals[greenCrystal.InternalID].ToString();
        else greenCrystalAmount.text = "0";

        // Update red crystals
        if (SaveSystem.saveData.crystals.ContainsKey(redCrystal.InternalID))
            redCrystalAmount.text = SaveSystem.saveData.crystals[redCrystal.InternalID].ToString();
        else redCrystalAmount.text = "0";
    }

    // Set the panel for a specific module
    public void SetPanel(ModuleData module)
    {
        // Set holder
        this.module = module;

        // Set panel colors
        moduleIcon.sprite = module.icon;
        moduleIcon.color = module.color;
        moduleBorderOne.color = module.color;
        moduleBorderTwo.color = module.color;
        panelBorder.color = module.color;
        panelButton.color = module.color;

        // Set panel info
        name.text = module.name;
        desc.text = module.desc;

        // Set amount owned
        if (SaveSystem.saveData.modules.ContainsKey(module.InternalID))
            owned.text = SaveSystem.saveData.modules[module.InternalID] + " OWNED";
        else owned.text = "NONE OWNED";
    }

    // Attempt to purchase the module
    public void PurchaseModule()
    {
        // Check if module is assigned
        if (module == null) return;

        // Check if player has enough crystals
        if (SaveSystem.saveData.crystals.ContainsKey(module.InternalID) &&
            SaveSystem.saveData.crystals[module.InternalID] > 0)
        {
            // Update the save with new module
            if (SaveSystem.saveData.modules.ContainsKey(module.InternalID))
                SaveSystem.saveData.modules[module.InternalID] += 1;
            else SaveSystem.saveData.modules.Add(module.InternalID, 1);
            SaveSystem.saveData.crystals[module.InternalID] -= 1;
            SaveSystem.UpdateSave();
            UpdateCrystals();
            SetPanel(module);

            // Debug
            Debug.Log("Successfully purchased " + module.name);
        } 
        else
        {
            Debug.Log("Player does not have enough crystals to buy " + module.name);
            return;
        }
    }
}
