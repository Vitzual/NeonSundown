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
    public TextMeshProUGUI desc, amount, purchased, unavailable;

    // Color settings
    public Color blueCrystal, redCrystal, greenCrystal;

    // Blackmarket data 
    [HideInInspector]
    public BlackmarketData data;
    private bool isUnlocked = false;

    // Set the item component
    public void Set(BlackmarketData data)
    {
        // Do nothing if null
        if (data == null || data.hideInExperimental)
        {
            amount.gameObject.SetActive(false);
            crystal.gameObject.SetActive(false);
            unavailable.gameObject.SetActive(true);
            return;
        }

        // Set data object
        this.data = data;

        // Set UI components
        name.text = data.name.ToUpper();
        desc.text = data.desc;
        icon.sprite = data.icon;

        // Set amount text
        if (SaveSystem.IsBlackmarketItemUnlocked(data.InternalID))
        {
            amount.gameObject.SetActive(false);
            crystal.gameObject.SetActive(false);
            purchased.gameObject.SetActive(true);
            purchased.color = data.lightColor;
            isUnlocked = true;
        }
        else
        {
            // Set amount text
            amount.text = data.amountRequired + " CRYSTALS";

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
        }

        // Set colors
        border.color = data.lightColor;
        background.color = data.darkColor;
        name.color = data.lightColor;

        // If light color set for icon, apply
        if (data.useLightColorOnIcon)
            icon.color = data.lightColor;
    }

    public void OnClick()
    {
        if (data != null && !isUnlocked)
            Events.active.BlackmarketItemClicked(data);
    }
}
