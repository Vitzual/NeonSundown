using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipButton : MonoBehaviour
{
    // Interface variables
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public Image icon, button;
    [HideInInspector]
    public ShipData ship;

    // Internal flags
    private bool isUnlocked;

    // Set the button
    public void Set(ShipData ship)
    {
        // Set the data
        this.ship = ship;

        // Check if ship is unlocked
        isUnlocked = ship.IsUnlocked();

        // If not locked, set variables
        if (isUnlocked)
        {
            name.text = ship.name;
            desc.text = ship.shortDesc;
            desc.color = ship.subColor;
            icon.sprite = ship.icon;
            button.color = ship.mainColor;
        }
        else desc.text = ship.unlockRequirement;

        // Set sibling index
        transform.SetSiblingIndex(ship.listOrder);
    }

    // Select ship
    public void SelectShip()
    {
        if (isUnlocked)
            Events.active.SetupShip(ship);
    }
}
