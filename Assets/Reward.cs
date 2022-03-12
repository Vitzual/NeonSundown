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
    public void Set(LevelData level, int levelNumber)
    {
        // Set level info
        levelText.text = levelNumber.ToString();
        xpAmount.text = level.xpRequirement.ToString();

        // Set reward info
        Color lightColor, darkColor;
        if (level.cardReward != null)
        {
            lightColor = level.cardReward.color;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            rewardIcon.sprite = level.cardReward.sprite;
            rewardName.text = level.cardReward.name;
            rewardDescription.text = level.cardReward.description;
            rewardIcon.color = lightColor;
        }
        else if (level.arenaReward != null)
        {
            lightColor = level.arenaReward.lightColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            rewardIcon.sprite = level.arenaReward.icon;
            rewardName.text = level.arenaReward.name;
            rewardDescription.text = level.arenaReward.shortDesc;
            rewardIcon.color = Color.white;
        }
        else if (level.shipReward != null)
        {
            lightColor = level.shipReward.mainColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            rewardIcon.sprite = level.shipReward.icon;
            rewardName.text = level.shipReward.name;
            rewardDescription.text = level.shipReward.shortDesc;
            rewardIcon.color = Color.white;
        }
        else
        {
            lightColor = level.levelColor;
            darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);

            rewardIcon.sprite = level.rewardIcon;
            rewardName.text = level.name;
            rewardDescription.text = level.desc;
            rewardIcon.color = level.rewardColor;
        }

        // Set colors
        border.color = lightColor;
        background.color = darkColor;
        xpAmount.color = darkColor;
        xpBackground.color = lightColor;
        rewardDescription.color = lightColor;
    }
}
