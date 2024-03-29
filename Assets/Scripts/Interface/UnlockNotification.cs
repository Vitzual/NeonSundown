using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockNotification : MonoBehaviour
{
    // Notification elements
    public NotificationManager notification;
    public Image background, border, levelIcon, xpBackground;
    public TextMeshProUGUI xpAmount, levelText;
    private AudioSource audioSource;
    public bool synergy = false;

    // On start subscribe to level up event
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (synergy) Events.active.onSynergyAvailable += SetSynergy;
        else Events.active.onLevelUp += Set;
    }

    // Set the synergy thing-a-ma-jig
    public void SetSynergy(SynergyData synergy)
    {
        // Set notification
        notification.icon = synergy.outputCard.sprite;
        notification.description = synergy.name;

        // Set colors
        notification.descriptionObj.color = synergy.outputCard.color;
        notification.iconObj.color = synergy.outputCard.color;
        border.color = synergy.outputCard.color;
        background.color = synergy.backgroundColor;

        // Display notification
        notification.UpdateUI();
        notification.OpenNotification();
        audioSource.volume = Settings.sound;
        audioSource.Play();
    }

    // Set level notification and display
    public void Set(LevelData level, int levelNumber)
    {
        // Set level info
        levelText.text = levelNumber.ToString();
        xpAmount.text = level.xpRequirement + "xp";

        // Set reward info
        Color lightColor, darkColor;
        if (level.cardReward != null)
        {
            lightColor = level.cardReward.color;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.cardReward.sprite;
            notification.title = level.cardReward.name.ToUpper();
            notification.description = "A NEW CARD IS NOW AVAILABLE!";
            notification.iconObj.color = lightColor;
        }
        else if (level.arenaReward != null)
        {
            lightColor = level.arenaReward.lightColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.arenaReward.enemyIcon;
            notification.title = level.arenaReward.name.ToUpper();
            notification.description = "A NEW ARENA IS NOW AVAILABLE!";
            notification.iconObj.color = lightColor;
        }
        else if (level.shipReward != null)
        {
            lightColor = level.shipReward.mainColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.shipReward.glowIcon;
            notification.title = level.shipReward.name.ToUpper();
            notification.description = "A NEW SHIP IS NOW AVAILABLE!";
            notification.iconObj.color = lightColor;
        }
        else if (level.synergyReward != null)
        {
            lightColor = level.synergyReward.outputCard.color;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.synergyReward.outputCard.sprite;
            notification.title = level.synergyReward.name.ToUpper();
            notification.description = "A NEW SYNERGY IS NOW AVAILABLE!";
            notification.iconObj.color = lightColor;
        }
        else
        {
            lightColor = level.levelColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.rewardIcon;
            notification.title = level.rewardName.ToUpper();
            notification.description = level.rewardDesc.ToUpper();
            if (level.IsColored()) notification.iconObj.color = level.rewardColor;
            else notification.iconObj.color = Color.white;
        }

        // Set colors
        levelIcon.color = lightColor;
        border.color = lightColor;
        background.color = darkColor;
        xpAmount.color = darkColor;
        xpBackground.color = lightColor;
        notification.descriptionObj.color = lightColor;
        notification.UpdateUI();
        notification.OpenNotification();
        audioSource.volume = Settings.sound;
        audioSource.Play();
    }
}
