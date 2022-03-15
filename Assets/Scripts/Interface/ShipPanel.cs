using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipPanel : MonoBehaviour
{
    // Module slot on ship panel
    [System.Serializable]
    public class Module
    {
        public Image module;
        public Image icon;
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
    private int moduleSlot = 0;
    public AudioClip moduleSound;
    private AudioSource audioSource;

    // Panel elements
    public new TextMeshProUGUI name;
    public TextMeshProUGUI modules, subTitle, desc, health, regen,
        speed, dash, damage, firerate, pierces, lifetime;
    private string healthStr, regenStr, speedStr, dashStr, 
        damageStr, firerateStr, piercesStr, lifetimeStr;
    public Image icon, leftLine, rightLine, bottomLine, panel,
        moduleCancelButton, moduleClearButton;

    // On start, subscribe to ship setup and create buttons
    public void Start()
    {
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

        // After creating, set ordering
        foreach (ShipButton button in buttons)
            button.gameObject.transform.SetSiblingIndex(button.ship.listOrder);

        // After creating, set ordering
        foreach (ShipButton button in buttons)
            button.gameObject.transform.SetSiblingIndex(button.ship.listOrder);
    }

    // Update the ships
    public void Setup(ShipData ship)
    {
        // Set modules to false
        ToggleModules(false);
        ClearModule(true);

        // Set the ship
        Gamemode.ship = ship;

        // Set all information
        name.text = ship.name;
        subTitle.text = ship.subTitle;
        desc.text = ship.desc;
        icon.sprite = ship.glowIcon;
        icon.color = ship.mainColor;

        // Set ship stats
        healthStr = "<b>HEALTH:</b><color=white> " + Formatter.Round(ship.startingHealth) + "hp";
        regenStr = "<b>REGEN:</b><color=white> " + Formatter.Round(ship.regenAmount) + " / second";
        speedStr = "<b>SPEED:</b><color=white> " + Formatter.Round(ship.playerSpeed) + " km/h";
        dashStr = "<b>DASH:</b><color=white> " + Formatter.Round(ship.dashSpeed) + " km/h";

        // Set weapon stats
        if (ship.weapon != null)
        {
            damageStr = "<b>DAMAGE:</b><color=white> " + Formatter.Round(ship.weapon.damage) + "hp / shot";
            firerateStr = "<b>COOLDOWN:</b><color=white> " + Formatter.Round(ship.weapon.cooldown) + " seconds";
            piercesStr = "<b>PIERCES:</b><color=white> " + Formatter.Round(ship.weapon.pierces) + " / shot";
            lifetimeStr = "<b>LIFETIME:</b><color=white> " + Formatter.Round(ship.weapon.lifetime) + " seconds";
        }
        else
        {
            damageStr = "<b>DAMAGE:</b><color=white> N/A";
            firerateStr = "<b>COOLDOWN:</b><color=white> N/A";
            piercesStr = "<b>PIERCES:</b><color=white> N/A";
            lifetimeStr = "<b>LIFETIME:</b><color=white> N/A";
        }

        // Set elements based off new strings
        health.text = healthStr;
        regen.text = regenStr;
        speed.text = speedStr;
        dash.text = dashStr;
        damage.text = damageStr;
        firerate.text = firerateStr;
        pierces.text = piercesStr;
        lifetime.text = lifetimeStr;

        // Set all colors
        panel.color = ship.mainColor;
        leftLine.color = ship.mainColor;
        rightLine.color = ship.mainColor;
        bottomLine.color = ship.mainColor;
        modules.color = ship.subColor;
        subTitle.color = ship.subColor;
        health.color = ship.subColor;
        regen.color = ship.subColor;
        speed.color = ship.subColor;
        dash.color = ship.subColor;
        damage.color = ship.subColor;
        firerate.color = ship.subColor;
        pierces.color = ship.subColor;
        lifetime.color = ship.subColor;
        //moduleCancelButton.color = ship.subColor;
        //moduleClearButton.color = ship.subColor;
    }

    // Open module list
    public void OpenModules(int slot)
    {
        // Update amounts based on save data
        foreach(KeyValuePair<string, int> module in SaveSystem.saveData.modules)
            if (moduleAmounts.ContainsKey(module.Key)) 
                moduleAmounts[module.Key].text = "OWNED";

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

        // Check if the module is valid 
        if (Gamemode.ship.weapon == null)
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

        // Is the module null
        bool isModuleNull = module == null;

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

                // Set module colors
                moduleSlots[moduleSlot].module.color = module.color;
                moduleSlots[moduleSlot].icon.sprite = module.icon;
                moduleSlots[moduleSlot].icon.color = module.color;

                // Play module sound
                Events.active.VolumeChanged(Settings.music / 3f);
                audioSource.volume = Settings.sound;
                audioSource.Play();
                StopAllCoroutines();
                StartCoroutine(MusicPlayer.FadeIn(1f, 2f));
            }
            else
            {
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
        UpdateModuleInterface(module.stat, false, module.value);
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
