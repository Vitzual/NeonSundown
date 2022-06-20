using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipPanel : MonoBehaviour
{
    // Is loading flag
    public static bool isLoading = false;

    // Module slot on ship panel
    [System.Serializable]
    public class Module
    {
        public Image module, icon;
        public TextMeshProUGUI name, amount;
    }
    public List<Module> moduleSlots;

    // Module buttons
    [System.Serializable]
    public class ModuleButton
    {
        public ModuleData data;
        public TextMeshProUGUI amount;
    }
    public List<ModuleButton> moduleButtons;
    private Dictionary<string, TextMeshProUGUI> moduleAmounts;
    private List<ModuleData> equippedModules;

    // Ship button prefab
    public ShipButton shipButton;
    public Transform shipList;
    public CanvasGroup shipCanvas;
    public CanvasGroup moduleCanvas;
    private List<ShipButton> buttons;
    [HideInInspector]
    public int moduleSlot = 0;
    public AudioClip moduleSound;
    private AudioSource audioSource;

    // Panel elements
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc, health, regen, speed, dash, 
        damage, firerate, pierces, lifetime;
    private string healthStr, regenStr, speedStr, dashStr, 
        damageStr, firerateStr, piercesStr, lifetimeStr;
    public Image icon, panelBackground, panelBorder, moduleCancelButton, moduleClearButton;

    // On start, subscribe to ship setup and create buttons
    public void Start()
    {
        // Set loading to false
        isLoading = false;

        // Get audio source component 
        audioSource = GetComponent<AudioSource>();

        // Setup module amounts
        equippedModules = new List<ModuleData>();
        moduleAmounts = new Dictionary<string, TextMeshProUGUI>();
        foreach (ModuleButton module in moduleButtons)
            moduleAmounts.Add(module.data.InternalID, module.amount);

        // Subscribe to setup event
        Events.active.onSetupShip += Setup;
        buttons = new List<ShipButton>();

        // Create ship buttons
        foreach (ShipData ship in Scriptables.ships)
        {
            ShipButton button = Instantiate(shipButton, Vector2.zero, Quaternion.identity);
            button.transform.SetParent(shipList);
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.localScale = new Vector3(1, 1, 1);
            button.Set(ship);
            buttons.Add(button);
        }

        // I'll come back to this later. Unity is being a dummy and
        // not giving two shits about setting the sibling index

        // After creating, set ordering
        foreach (ShipButton button in buttons)
            button.gameObject.transform.SetSiblingIndex(button.ship.listOrder);
    }

    // Update the ships
    public void Setup(ShipData ship)
    {
        // Set modules to false
        //ToggleModules(false);
        //ClearModule(true);

        // Set the ship
        Gamemode.ship = ship;

        // Set all information
        name.text = ship.name.ToUpper() + " <size=18><color=#" + ColorUtility.ToHtmlStringRGB(
            ship.lightColor) + ">" + ship.subTitle;
        desc.text = ship.desc;
        icon.sprite = ship.glowIcon;
        icon.color = ship.mainColor;
        panelBackground.color = ship.darkColor;
        panelBorder.color = ship.mainColor;
    }

    // Open module list
    public void OpenModules(int slot)
    {
        // Update amounts based on save data
        foreach (KeyValuePair<string, int> module in SaveSystem.saveData.modules)
        {
            int amount = SaveSystem.GetModuleAmount(module.Key);
            if (amount == -1) moduleAmounts[module.Key].text = "NOT OWNED";
            else if (amount == 4) moduleAmounts[module.Key].text = "LEVEL MAX";
            else moduleAmounts[module.Key].text = "LEVEL " + (amount + 1);
        }

        // Open modules and set slot
        moduleSlot = slot;
        ToggleModules(true);
    }

    // Clear module
    public void ClearModule(bool all)
    {
        if (all)
        {
            for (int i = 0; i < moduleSlots.Count; i++)
            {
                moduleSlot = i;
                SetModule(null);
            }
        }
        else SetModule(null);
    }

    // Set a module
    public void SetModule(ModuleData module)
    {
        // Check if ship is null
        if (Gamemode.ship == null) return;

        // Is the module null
        bool isModuleNull = module == null;

        // Check if the module is valid 
        if (Gamemode.ship.weapon == null && !isModuleNull)
        {
            if (module.stat == Stat.Damage || module.stat == Stat.Range || module.stat == Stat.Cooldown ||
                module.stat == Stat.Bullets || module.stat == Stat.Pierces || module.stat == Stat.Lifetime)
            {
                Debug.Log(module.name + " cannot be applied to " + Gamemode.ship.name + "!");
                return;
            }
        }

        // Check if module is already applied
        if (equippedModules.Contains(module)) return;

        // Check if user has enough modules
        if (isModuleNull || SaveSystem.HasModule(module.InternalID))
        {
            // Check if module is already added
            if (Gamemode.modules.ContainsKey(moduleSlot))
            {
                if (Gamemode.modules[moduleSlot] != null)
                {
                    UpdateModuleInterface(Gamemode.modules[moduleSlot].stat, true, 0);
                    if (equippedModules.Contains(Gamemode.modules[moduleSlot]))
                        equippedModules.Remove(Gamemode.modules[moduleSlot]);
                }
                Gamemode.modules[moduleSlot] = module;
            }
            else Gamemode.modules.Add(moduleSlot, module);

            // Update the module slot
            if (!isModuleNull)
            {
                // Remove module from player and apply
                ApplyModule(module);
                equippedModules.Add(module);

                // Set module names
                moduleSlots[moduleSlot].name.text = module.name.ToUpper();
                moduleSlots[moduleSlot].amount.text = module.values[SaveSystem.GetModuleAmount(module.InternalID)].ToString();
                if (module.multi) moduleSlots[moduleSlot].amount.text += "x";

                // Set module colors
                moduleSlots[moduleSlot].module.color = module.color;
                moduleSlots[moduleSlot].icon.sprite = module.icon;
                moduleSlots[moduleSlot].icon.color = module.color;

                // Play module sound
                if (!isLoading)
                {
                    Events.active.VolumeChanged(Settings.music / 3f);
                    audioSource.volume = Settings.sound;
                    audioSource.Play();
                    StopAllCoroutines();
                    StartCoroutine(MusicPlayer.FadeIn(1f, 2f));
                }
            }
            else
            {
                moduleSlots[moduleSlot].name.text = "EMPTY";
                moduleSlots[moduleSlot].amount.text = "SLOT";
                moduleSlots[moduleSlot].module.color = Color.white;
                moduleSlots[moduleSlot].icon.color = new Color(0, 0, 0, 0);
            }

            // Toggle modules
            ToggleModules(false);
        }
        else ToggleModules(false);
    }
    
    // Applies a module to the ship stats
    private void ApplyModule(ModuleData module)
    {
        // Check if stat already exists and apply
        if (Gamemode.modules.ContainsKey(moduleSlot))
            Gamemode.modules[moduleSlot] = module;
        else Gamemode.modules.Add(moduleSlot, module);
        UpdateModuleInterface(module.stat, false, module.values[SaveSystem.GetModuleAmount(module.InternalID)]);
    }

    // Toggle module list
    public void ToggleModules(bool toggle)
    {
        if (toggle)
        {
            shipCanvas.alpha = 0f;
            shipCanvas.interactable = false;
            shipCanvas.blocksRaycasts = false;

            moduleCanvas.alpha = 1f;
            moduleCanvas.interactable = true;
            moduleCanvas.blocksRaycasts = true;
        }
        else
        {
            moduleCanvas.alpha = 0f;
            moduleCanvas.interactable = false;
            moduleCanvas.blocksRaycasts = false;

            shipCanvas.alpha = 1f;
            shipCanvas.interactable = true;
            shipCanvas.blocksRaycasts = true;
        }
    }

    // Update interface
    public void UpdateModuleInterface(Stat stat, bool clear, float val)
    {
        // Check if stat is no longer applied
        ShipData ship = Gamemode.ship;

        // Update the UI elements
        switch (stat)
        {
            case Stat.Health:
                if (clear) health.text = healthStr;
                else health.text = healthStr + "<color=green> (+" + Formatter.Round(
                    (ship.startingHealth + val) - ship.startingHealth) + ")";
                break;
            case Stat.Regen:
                if (clear) regen.text = regenStr;
                else regen.text = regenStr + "<color=green> (+" + val + ")";
                break;
            case Stat.MoveSpeed:
                if (clear) speed.text = speedStr;
                else speed.text = speedStr + "<color=green> (+" + Formatter.Round(
                    (ship.playerSpeed * val) - ship.playerSpeed) + ")";
                break;
            case Stat.Damage:
                if (clear) damage.text = damageStr;
                else damage.text = damageStr + "<color=green> (+" + Formatter.Round(
                    (ship.weapon.damage * val) - ship.weapon.damage) + ")";
                break;
            case Stat.Cooldown:
                if (clear) firerate.text = firerateStr;
                else firerate.text = firerateStr + "<color=green> (-" + Formatter.Round(
                    ship.weapon.cooldown - (ship.weapon.cooldown * val)) + ")";
                break;
            case Stat.Pierces:
                if (clear) pierces.text = piercesStr;
                else pierces.text = piercesStr + "<color=green> (+" + val + ")";
                break;
            case Stat.Lifetime:
                if (clear) lifetime.text = lifetimeStr;
                else lifetime.text = lifetimeStr + "<color=green> (+" + Formatter.Round(
                    (ship.weapon.lifetime * val) - ship.weapon.lifetime) + ")";
                break;
        }
    }
}
