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

    // Ship button prefab
    public ShipButton shipButton;
    public Transform shipList;
    public CanvasGroup shipCanvas;
    public CanvasGroup moduleCanvas;
    private List<ShipButton> buttons;
    private int moduleSlot = 0;

    // Panel elements
    public new TextMeshProUGUI name;
    public TextMeshProUGUI modules, subTitle, desc, health, regen,
        speed, dash, damage, firerate, pierces, lifetime;
    public Image icon, leftLine, rightLine, bottomLine, panel;

    // On start, subscribe to ship setup and create buttons
    public void Start()
    {
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

        // After creating, set ordering
        foreach (ShipButton button in buttons)
            button.transform.SetSiblingIndex(button.ship.listOrder);
    }

    // Update the ships
    public void Setup(ShipData ship)
    {
        // Set modules to false
        ToggleModules(false);

        // Set the ship
        Gamemode.ship = ship;

        // Set all information
        name.text = ship.name;
        subTitle.text = ship.subTitle;
        desc.text = ship.desc;
        icon.sprite = ship.icon;

        // Set ship stats
        health.text = "<b>HEALTH:</b><color=white> " + Formatter.Round(ship.startingHealth) + "hp";
        regen.text = "<b>REGEN:</b><color=white> " + Formatter.Round(ship.regenRate) + " / second";
        speed.text = "<b>SPEED:</b><color=white> " + Formatter.Round(ship.playerSpeed) + " km/h";
        dash.text = "<b>DASH:</b><color=white> " + Formatter.Round(ship.dashSpeed) + " km/h";

        // Set weapon stats
        if (ship.weapon != null)
        {
            damage.text = "<b>DAMAGE:</b><color=white> " + Formatter.Round(ship.weapon.damage) + "hp / shot";
            firerate.text = "<b>FIRERATE:</b><color=white> " + Formatter.Round(ship.weapon.cooldown / 1f) + " / second";
            pierces.text = "<b>PIERCES:</b><color=white> " + Formatter.Round(ship.weapon.pierces) + " / shot";
            lifetime.text = "<b>LIFETIME:</b><color=white> " + Formatter.Round(ship.weapon.lifetime) + " seconds";
        }
        else
        {
            damage.text = "<b>DAMAGE:</b><color=white> N/A";
            firerate.text = "<b>FIRERATE:</b><color=white> N/A";
            pierces.text = "<b>PIERCES:</b><color=white> N/A";
            lifetime.text = "<b>LIFETIME:</b><color=white> N/A";
        }

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
    }

    // Open module list
    public void OpenModules(int slot)
    {
        moduleSlot = slot;
        ToggleModules(true);
    }

    // Set a module
    public void SetModule(ModuleData module)
    {
        // Is the module null
        bool isModuleNull = module == null;

        // Check if user has enough modules
        if (isModuleNull || SaveSystem.HasModule(module.InternalID))
        {
            // Check if module is already added
            if (Gamemode.modules.ContainsKey(moduleSlot))
            {
                if (Gamemode.modules[moduleSlot] != null)
                    SaveSystem.AddModule(Gamemode.modules[moduleSlot].InternalID, 1);
                Gamemode.modules[moduleSlot] = module;
            }
            else Gamemode.modules.Add(moduleSlot, module);

            // Update the module slot
            if (!isModuleNull)
            {
                moduleSlots[moduleSlot].module.color = module.color;
                moduleSlots[moduleSlot].icon.sprite = module.icon;
                moduleSlots[moduleSlot].icon.color = module.color;
            }
            else
            {
                moduleSlots[moduleSlot].module.color = Color.white;
                moduleSlots[moduleSlot].icon.color = new Color(0, 0, 0, 0);
            }

            // Toggle modules
            ToggleModules(false);
        }
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
}
