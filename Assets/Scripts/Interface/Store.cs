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
    public TextMeshProUGUI buttonText;
    public AudioClip purchaseSound;

    // Crystal variables
    public CrystalData blueCrystal;
    public TextMeshProUGUI blueCrystalAmount;
    public CrystalData greenCrystal;
    public TextMeshProUGUI greenCrystalAmount;
    public CrystalData redCrystal;
    public TextMeshProUGUI redCrystalAmount;

    // Internal variables
    private ModuleData module;
    private AudioSource audioSource;
    
    // Get the audio source on this object
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = purchaseSound;
    }

    // Update the crystal amounts
    public void UpdateCrystals()
    {
        // Update blue crystals
        int amount;
        if (SaveSystem.saveData.crystals.ContainsKey(blueCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[blueCrystal.InternalID];
            if (amount > 99) blueCrystalAmount.text = "99+";
            else blueCrystalAmount.text = amount.ToString();
        }
        else blueCrystalAmount.text = "0";

        // Update green crystals
        if (SaveSystem.saveData.crystals.ContainsKey(greenCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[greenCrystal.InternalID];
            if (amount > 99) greenCrystalAmount.text = "99+";
            else greenCrystalAmount.text = amount.ToString();
        }
        else greenCrystalAmount.text = "0";

        // Update red crystals
        if (SaveSystem.saveData.crystals.ContainsKey(redCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[redCrystal.InternalID];
            if (amount > 99) redCrystalAmount.text = "99+";
            else redCrystalAmount.text = amount.ToString();
        }
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
        owned.color = module.color;

        // Get value
        float value = module.values[0];

        // Set amount owned
        if (SaveSystem.saveData.modules.ContainsKey(module.InternalID))
        {
            int amount = SaveSystem.GetModuleAmount(module.InternalID);
            value = module.values[amount];
            if (amount > 2)
            {
                owned.text = "MAX LEVEL";
                buttonText.text = "MAX LEVEL";
            }
            else
            {
                owned.text = module.GetCost(amount) + "x Crystals";
                buttonText.text = "UPGRADE";
            }
        }
        else
        {
            owned.text = module.GetCost(-1) + "x Crystals";
            buttonText.text = "PURCHASE";
        }

        // Set panel info
        name.text = module.name;
        desc.text = module.desc.Replace("{value}", value.ToString());
    }

    // Attempt to purchase the module
    public void PurchaseModule()
    {
        // Check if module is assigned
        if (module == null) return;

        // Check if player owns module
        if (SaveSystem.GetModuleAmount(module.InternalID) > 2) return;

        // Check if player has enough crystals
        if (SaveSystem.GetCrystalAmount(module.cost.InternalID) >= 
            module.GetCost(SaveSystem.GetModuleAmount(module.InternalID)))
        {
            // Update the save with new module
            SaveSystem.AddCrystal(module.cost.InternalID, -module.GetCost(SaveSystem.GetModuleAmount(module.InternalID)));
            SaveSystem.AddModule(module.InternalID);
            SaveSystem.UpdateSave();
            UpdateCrystals();
            SetPanel(module);

            // Play sound
            Events.active.VolumeChanged(Settings.music / 3f);
            audioSource.volume = Settings.sound;
            audioSource.Play();
            StopAllCoroutines();
            StartCoroutine(MusicPlayer.FadeIn(1f, 2f));

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
