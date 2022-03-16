using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyProgress : MonoBehaviour
{
    public TextMeshProUGUI synergyName, desc, status, requirementOne, 
        collectedOne, requirementTwo, collectedTwo;
    public Image icon, border, background, statusBackground, iconOne,
        iconTwo, barOne, barBackgroundOne, barTwo, barBackgroundTwo;
    public ProgressBar cardBarOne, cardBarTwo;

    public void Set(SynergyData synergy)
    {
        // Set text objects
        synergyName.text = synergy.name;
        desc.text = synergy.outputCard.description;
        if (Deck.active.HasCard(synergy.outputCard)) status.text = "ACTIVE";
        else if (SynergyHandler.availableSynergies.Contains(synergy)) status.text = "READY";
        else status.text = "NOT READY";
        requirementOne.text = synergy.cardOne.name;
        requirementTwo.text = synergy.cardTwo.name;
        collectedOne.text = Deck.active.GetCardAmount(synergy.cardOne) + 
            "/" + synergy.cardOne.maximumAmount + " COLLECTED";
        collectedTwo.text = Deck.active.GetCardAmount(synergy.cardTwo) +
            "/" + synergy.cardTwo.maximumAmount + " COLLECTED";

        // Set the progress bars
        cardBarOne.currentPercent = (float)Deck.active.GetCardAmount(synergy.cardOne) 
            / (float)synergy.cardOne.maximumAmount;
        cardBarTwo.currentPercent = (float)Deck.active.GetCardAmount(synergy.cardTwo)
            / (float)synergy.cardTwo.maximumAmount;
        cardBarOne.UpdateUI();
        cardBarTwo.UpdateUI();

        // Set image components
        icon.sprite = synergy.achievementIcon;
        iconOne.sprite = synergy.cardOne.sprite;
        iconTwo.sprite = synergy.cardTwo.sprite;

        // Set color components
        desc.color = synergy.primaryColor;
        background.color = synergy.backgroundColor;
        border.color = synergy.primaryColor;
        statusBackground.color = synergy.primaryColor;
        iconOne.color = synergy.cardOne.color;
        iconTwo.color = synergy.cardTwo.color;

        // Set bar colors
        Color c = synergy.cardOne.color;
        barBackgroundOne.color = new Color(c.r, c.g, c.b, 0.2f);
        barOne.color = c;
        c = synergy.cardTwo.color;
        barBackgroundTwo.color = new Color(c.r, c.g, c.b, 0.2f);
        barTwo.color = c;
    }
}
