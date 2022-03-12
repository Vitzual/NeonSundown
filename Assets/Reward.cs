using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    // Interface variables
    public Image levelIcon, border, background, rewardIcon, xpBackground;
    public TextMeshProUGUI levelText, rewardName, rewardDescription, xpAmount;

    // Set a reward
    public void Set(LevelData level, int levelNumber, bool unlocked)
    {
        // Set level info
        levelText.text = levelNumber.ToString();
        xpAmount.text = level.xpRequirement + "xp";

        // Set reward info
        Color lightColor = Color.gray;
        Color darkColor = new Color(lightColor.r * 0.2f, 
            lightColor.g * 0.2f, lightColor.b * 0.2f);

        // Check if locked
        if (!unlocked) rewardName.color = lightColor;

        if (level.cardReward != null)
        {
            if (unlocked)
            {
                lightColor = level.cardReward.color;
                darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);
            }

            rewardIcon.sprite = level.cardReward.sprite;
            rewardName.text = level.cardReward.name + " Card";
            rewardDescription.text = level.cardReward.description;
            rewardIcon.color = lightColor;
        }
        else if (level.arenaReward != null)
        {
            if (unlocked)
            {
                lightColor = level.arenaReward.lightColor;
                darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);
            }

            rewardIcon.sprite = level.arenaReward.iconEnemy;
            rewardName.text = level.arenaReward.name;
            rewardDescription.text = level.arenaReward.shortDesc;
            if (unlocked) rewardIcon.color = lightColor;
            else rewardIcon.color = Color.gray;
        }
        else if (level.shipReward != null)
        {
            if (unlocked)
            {
                lightColor = level.shipReward.mainColor;
                darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);
            }

            rewardIcon.sprite = level.shipReward.glowIcon;
            rewardName.text = level.shipReward.name;
            rewardDescription.text = level.shipReward.shortDesc;
            if (unlocked) rewardIcon.color = lightColor;
            else rewardIcon.color = Color.gray;
        }
        else
        {
            if (unlocked)
            {
                lightColor = level.levelColor;
                darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);
            }

            rewardIcon.sprite = level.rewardIcon;
            rewardName.text = level.name;
            rewardDescription.text = level.desc;
            if (unlocked) rewardIcon.color = level.rewardColor;
            else rewardIcon.color = Color.gray;
        }

        // Set colors
        levelIcon.color = lightColor;
        border.color = lightColor;
        background.color = darkColor;
        xpAmount.color = darkColor;
        xpBackground.color = lightColor;
        rewardDescription.color = lightColor;
    }
}
