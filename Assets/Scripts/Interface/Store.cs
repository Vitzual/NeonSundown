using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    // List of level icons 
    [System.Serializable]
    public class ModuleLevels
    {
        public ModuleData module;
        public List<Image> levelIcon;
    }
    public List<ModuleLevels> moduleLevels;

    // Interface variables
    public Image moduleIcon;
    public Image moduleBorderOne;
    public Image moduleBorderTwo;
    public Image panelBorder;
    public Image panelBackground;
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

        // Setup level icons
        foreach(ModuleLevels level in moduleLevels)
        {
            int amount = SaveSystem.GetModuleAmount(level.module.InternalID);
            int totalColored = 0;
            for (int i = 0; i <= amount; i++)
            {
                level.levelIcon[i].color = level.module.color;
                totalColored += 1;
            }
            for (int i = totalColored; i < level.levelIcon.Count; i++)
                level.levelIcon[i].color = level.module.cost.darkColor;
        }
    }

    // Update the crystal amounts
    public void UpdateCrystals()
    {
        // Update blue crystals
        int amount;
        if (SaveSystem.saveData.crystals.ContainsKey(blueCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[blueCrystal.InternalID];
            if (amount > 999) blueCrystalAmount.text = "999";
            else blueCrystalAmount.text = amount.ToString();
        }
        else blueCrystalAmount.text = "0";

        // Update green crystals
        if (SaveSystem.saveData.crystals.ContainsKey(greenCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[greenCrystal.InternalID];
            if (amount > 999) greenCrystalAmount.text = "999";
            else greenCrystalAmount.text = amount.ToString();
        }
        else greenCrystalAmount.text = "0";

        // Update red crystals
        if (SaveSystem.saveData.crystals.ContainsKey(redCrystal.InternalID))
        {
            amount = SaveSystem.saveData.crystals[redCrystal.InternalID];
            if (amount > 999) redCrystalAmount.text = "999";
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
        panelBackground.color = new Color(module.color.r, module.color.g, module.color.b, 0.25f);
        panelButton.color = module.color;
        owned.color = module.color;

        // Get value
        float value = module.values[0];

        // Set amount owned
        if (SaveSystem.saveData.modules.ContainsKey(module.InternalID))
        {
            int amount = SaveSystem.GetModuleAmount(module.InternalID);
            value = module.values[amount];
            if (amount > 3)
            {
                owned.text = "MAX LEVEL";
                buttonText.text = "MAX LEVEL";
            }
            else
            {
                owned.text = module.GetCost(amount) + "x CRYSTALS";
                buttonText.text = "UPGRADE";
            }
        }
        else
        {
            owned.text = module.GetCost(-1) + "x CRYSTALS";
            buttonText.text = "PURCHASE";
        }

        // Set panel info
        name.text = module.name.ToUpper();
        desc.text = module.desc.Replace("{value}", value.ToString());
    }
    
    // Attempt to purchase the module
    public void PurchaseModule()
    {
        // Check if module is assigned
        if (module == null) return;

        // Check if player owns module
        if (SaveSystem.GetModuleAmount(module.InternalID) > 3) return;

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
            UpdateLevels(module);

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

    // Updates a specific module
    public void UpdateLevels(ModuleData module)
    {
        // Setup level icons
        foreach (ModuleLevels level in moduleLevels)
        {
            if (level.module == module)
            {
                int amount = SaveSystem.GetModuleAmount(module.InternalID);
                int totalColored = 0;
                for (int i = 0; i <= amount; i++)
                {
                    level.levelIcon[i].color = module.color;
                    totalColored += 1;
                }
                for (int i = totalColored; i < level.levelIcon.Count; i++)
                    level.levelIcon[i].color = module.cost.darkColor;
                return;
            }
        }
    }
}
