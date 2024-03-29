using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipButton : MonoBehaviour
{
    // Interface variables
    [Header("Button Variables")]
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public Image shipIcon, shipBackground, shipBorder, buttonBorder, buttonBackground;
    [HideInInspector]
    public ShipData ship;
    public OnHoverAdjustScale onHoverAdjustScale;

    // Options for the button
    [Header("Button Options")]
    public Color lockedDescColor;
    public Color lockedBorderColor;
    public Color lockedBackgroundColor;
    public Color lockedShipColor;
    public Color lockedShipBackgroundColor;

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
            name.text = ship.name.ToUpper();
            desc.text = ship.subTitle.ToUpper();
            desc.color = ship.lightColor;
            shipIcon.sprite = ship.glowIcon;
            shipIcon.color = ship.mainColor;
            shipBackground.color = ship.veryDarkColor;
            shipBorder.color = ship.mainColor;
            buttonBorder.color = ship.mainColor;
            buttonBackground.color = ship.darkColor;
            onHoverAdjustScale.enabled = true;
        }
        else
        {
            name.text = "LOCKED";
            desc.text = ship.unlockRequirement.ToUpper();
            desc.color = lockedDescColor;
            shipIcon.sprite = ship.glowIcon;
            shipIcon.color = Color.white;
            shipBackground.color = lockedShipBackgroundColor;
            shipBorder.color = lockedBorderColor;
            buttonBorder.color = lockedBorderColor;
            buttonBackground.color = lockedBackgroundColor;
            onHoverAdjustScale.enabled = false;
        }

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
