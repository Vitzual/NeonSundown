using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackmarketItem : MonoBehaviour
{
    // Item UI components
    public Image border, background, icon, crystal;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc, amount;

    // Color settings
    public Color blueCrystal, redCrystal, greenCrystal;

    // Set the item component
    public void Set(BlackmarketData data)
    {
        // Set UI components
        name.text = data.name.ToUpper();
        desc.text = data.desc;
        icon.sprite = data.icon;
        amount.text = data.amountRequired + " CRYSTALS";

        // Set colors
        border.color = data.lightColor;
        background.color = data.darkColor;
        name.color = data.lightColor;

        // Set crystal color
        switch (data.crystal)
        {
            case CrystalType.blue:
                crystal.color = blueCrystal;
                break;
            case CrystalType.green:
                crystal.color = greenCrystal;
                break;
            case CrystalType.red:
                crystal.color = redCrystal;
                break;
        }

        // If light color set for icon, apply
        if (data.useLightColorOnIcon)
            icon.color = data.lightColor;
    }
}
