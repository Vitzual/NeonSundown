using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyProgress : MonoBehaviour
{
    public GameObject locked, unlocked, requirementOneLock, requirementTwoLock;
    public TextMeshProUGUI synergyName, desc, status, requirementOne, 
        collectedOne, requirementTwo, collectedTwo, unlockReq;
    public Image icon, border, background, statusBackground, iconOne,
        iconTwo, barOne, barBackgroundOne, barTwo, barBackgroundTwo;
    public ProgressBar cardBarOne, cardBarTwo;
    [HideInInspector] public SynergyData data;

    public void Set(SynergyData synergy)
    {
        // Check if synergy is null
        if (synergy == null) return;

        // Set the synergy data
        data = synergy;

        // Check if synergy unlocked
        if (!SaveSystem.IsSynergyUnlocked(synergy.InternalID))
        {
            // Set locked to true
            unlocked.SetActive(false);
            locked.SetActive(true);

            // Find unlock requirement
            foreach (LevelData level in Levels.ranks)
                if (level.synergyReward != null && level.synergyReward.InternalID == synergy.InternalID)
                    unlockReq.text = "UNLOCKED AT RANK " + level.name;
        }
        else
        {
            // Set locked to false
            unlocked.SetActive(true);
            locked.SetActive(false);

            // Set text objects
            synergyName.text = synergy.name.ToString().ToUpper();
            desc.text = synergy.outputCard.description;
            if (Deck.active.HasCard(synergy.outputCard)) status.text = "ACTIVE";
            else if (SynergyHandler.availableSynergies.Contains(synergy)) status.text = "READY";
            else status.text = "NOT READY";
            requirementOne.text = synergy.cardOne.name.ToUpper();
            requirementTwo.text = synergy.cardTwo.name.ToUpper();
            int amountCollected = Deck.active.GetCardAmount(synergy.cardOne);

            // Set amount collected
            if (amountCollected < 0) amountCollected = 0;
            collectedOne.text = amountCollected + "/" + synergy.cardOne.maximumAmount + " COLLECTED";
            amountCollected = Deck.active.GetCardAmount(synergy.cardTwo);
            if (amountCollected < 0) amountCollected = 0;
            collectedTwo.text = amountCollected + "/" + synergy.cardTwo.maximumAmount + " COLLECTED";

            // Set the progress bars
            cardBarOne.currentPercent = (float)Deck.active.GetCardAmount(synergy.cardOne)
                / (float)synergy.cardOne.maximumAmount;
            cardBarTwo.currentPercent = (float)Deck.active.GetCardAmount(synergy.cardTwo)
                / (float)synergy.cardTwo.maximumAmount;
            cardBarOne.UpdateUI();
            cardBarTwo.UpdateUI();

            // Check for blacklist cards
            if (!Deck.active.HasCard(synergy.outputCard))
            {
                if (Gamemode.blacklistCards.Contains(synergy.cardOne))
                {
                    requirementOneLock.SetActive(true);
                    status.text = "UNAVAILABLE";
                }
                if (Gamemode.blacklistCards.Contains(synergy.cardTwo))
                {
                    requirementTwoLock.SetActive(true);
                    status.text = "UNAVAILABLE";
                }
            }

            // Set image components
            icon.sprite = synergy.outputCard.sprite;
            iconOne.sprite = synergy.cardOne.sprite;
            iconTwo.sprite = synergy.cardTwo.sprite;

            // Set color components
            icon.color = synergy.outputCard.color;
            status.color = synergy.backgroundColor;
            desc.color = synergy.outputCard.color;
            background.color = synergy.backgroundColor;
            border.color = synergy.outputCard.color;
            statusBackground.color = synergy.outputCard.color;
            iconOne.color = synergy.cardOne.color;
            iconTwo.color = synergy.cardTwo.color;

            // Set bar colors
            Color c = synergy.cardOne.color;
            barBackgroundOne.color = new Color(c.r - 0.5f, c.g - 0.5f, c.b - 0.5f);
            barOne.color = c;
            c = synergy.cardTwo.color;
            barBackgroundTwo.color = new Color(c.r - 0.5f, c.g - 0.5f, c.b - 0.5f);
            barTwo.color = c;
        }
    }
}
