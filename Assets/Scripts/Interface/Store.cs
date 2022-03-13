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
        owned.color = module.color;

        // Set panel info
        name.text = module.name;
        desc.text = module.desc;

        // Set amount owned
        if (SaveSystem.saveData.modules.ContainsKey(module.InternalID))
        {
            owned.text = "LEVEL " + SaveSystem.GetModuleAmount(module.InternalID);
            buttonText.text = "UPGRADE";
        }
        else
        {
            owned.text = "10x Crystals";
            buttonText.text = "PURCHASE";
        }
    }

    // Attempt to purchase the module
    public void PurchaseModule()
    {
        // Check if module is assigned
        if (module == null) return;

        // Check if player owns module
        if (SaveSystem.HasModule(module.InternalID)) return;

        // Check if player has enough crystals
        if (SaveSystem.GetCrystalAmount(module.cost.InternalID) > 10)
        {
            // Update the save with new module
            SaveSystem.AddModule(module.InternalID, 1);
            SaveSystem.AddCrystal(module.cost.InternalID, -1);
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
