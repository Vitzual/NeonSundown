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
    private bool isLocked;

    // Set the button
    public void Set(ShipData ship)
    {
        // Set the data
        this.ship = ship;

        // Check if ship is unlocked
        isLocked = SaveSystem.IsShipUnlocked(ship.InternalID);

        // If not locked, set variables
        if (!isLocked)
        {
            name.text = ship.name;
            desc.text = ship.shortDesc;
            desc.color = ship.subColor;
            icon.sprite = ship.icon;
            button.color = ship.mainColor;
        }
    }

    // Select ship
    public void SelectShip()
    {
        if (!isLocked)
            Events.active.SetupShip(ship);
    }
}
