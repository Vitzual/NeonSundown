using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanningPanel : MonoBehaviour
{
    // Ship buttons
    [System.Serializable]
    public class ShipButton
    {
        public ShipData data;
        public Image image;
        public Button button;
        public OnHoverAdjustScale scale;
    }

    // List of all ships
    public List<ShipButton> ships;
    public Sprite lockedShipIcon;
    public Color selectedColor;
    public Color unselectedColor;

    // Update the ships
    public void Setup()
    {
        foreach(ShipButton ship in ships)
        {
            // Default ship flag
            bool setDefault = Gamemode.ship == null;

            // Iterate through ships and set sprite
            //if (ship.data.IsUnlocked())
            //{
                // Set the ship variables
                ship.image.sprite = ship.data.model;
                ship.button.enabled = true;
                ship.scale.enabled = true;

                if (setDefault && ship.data.unlocked) ChangeShip(ship.data);
            //}

            // If ship not unlocked, set the locked image
            //else
            //{
                // Set the ship variables
                ship.image.sprite = lockedShipIcon;
                ship.image.color = unselectedColor;
                ship.button.enabled = false;
                ship.scale.enabled = false;
            //}
        }
    }

    // Update is called once per frame
    public void ChangeShip(ShipData data)
    {
        // Set the ship in the gamemode
        Gamemode.ship = data;

        // Toggle ship buttons
        foreach(ShipButton ship in ships)
        {
            if (ship.data == data) ship.image.color = selectedColor;
            else ship.image.color = unselectedColor;
        }
    }
}
