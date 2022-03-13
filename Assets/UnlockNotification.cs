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
    public TextMeshProUGUI xpAmount, levelText, desc;
    private AudioSource audioSource;

    // On start subscribe to level up event
    public void Start() { Events.active.onLevelUp += Set; audioSource = GetComponent<AudioSource>(); }

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
            notification.title = level.cardReward.name + " Card";
            notification.description = level.cardReward.description;
            notification.iconObj.color = lightColor;
        }
        else if (level.arenaReward != null)
        {
            lightColor = level.arenaReward.lightColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.arenaReward.iconEnemy;
            notification.title = level.arenaReward.name + " Arena";
            notification.description = level.arenaReward.shortDesc;
            notification.iconObj.color = lightColor;
        }
        else if (level.shipReward != null)
        {
            lightColor = level.shipReward.mainColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.shipReward.glowIcon;
            notification.title = level.shipReward.name + " Ship";
            notification.description = level.shipReward.shortDesc;
            notification.iconObj.color = lightColor;
        }
        else
        {
            lightColor = level.levelColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            notification.icon = level.rewardIcon;
            notification.title = level.rewardName;
            notification.description = level.rewardDesc;
            notification.iconObj.color = level.rewardColor;
        }

        // Set colors
        notification.iconObj.color = lightColor;
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
