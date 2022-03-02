using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipPanel : MonoBehaviour
{
    // Ship button prefab
    public ShipButton shipButton;
    public Transform shipList;
    private List<ShipButton> buttons;

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
        // Set the ship
        Gamemode.ship = ship;

        // Set all information
        name.text = ship.name;
        subTitle.text = ship.subTitle;
        desc.text = ship.desc;
        icon.sprite = ship.icon;

        // Set ship stats
        health.text = "<b>HEALTH:</b><color=white>" + Formatter.Round(ship.startingHealth) + "hp";
        regen.text = "<b>REGEN:</b><color=white>" + Formatter.Round(ship.regenRate) + " / second";
        speed.text = "<b>SPEED:</b><color=white>" + Formatter.Round(ship.playerSpeed) + " km/h";
        dash.text = "<b>DASH:</b><color=white>" + Formatter.Round(ship.dashSpeed) + " km/h";

        // Set weapon stats
        if (ship.weapon != null)
        {
            damage.text = "<b>DAMAGE:</b><color=white>" + Formatter.Round(ship.weapon.damage) + "hp / shot";
            firerate.text = "<b>FIRERATE:</b><color=white>" + Formatter.Round(ship.weapon.cooldown / 1f) + " / second";
            pierces.text = "<b>PIERCES:</b><color=white>" + Formatter.Round(ship.weapon.pierces) + " / shot";
            lifetime.text = "<b>LIFETIME:</b><color=white>" + Formatter.Round(ship.weapon.lifetime) + " seconds";
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
}
