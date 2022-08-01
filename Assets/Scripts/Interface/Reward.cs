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
    public void Set(LevelData level, int levelNumber, bool unlocked, Sprite levelIcon)
    {
        // Set level info
        this.levelIcon.sprite = levelIcon;
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
            rewardName.text = level.cardReward.name.ToUpper();
            rewardDescription.text = "UNLOCKS A NEW CARD TO FIND IN RUNS";
            rewardIcon.color = lightColor;
        }
        else if (level.arenaReward != null)
        {
            if (unlocked)
            {
                lightColor = level.arenaReward.lightColor;
                darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);
            }

            rewardIcon.sprite = level.arenaReward.enemyIcon;
            rewardName.text = level.arenaReward.name.ToUpper();
            rewardDescription.text = "UNLOCKS A NEW ARENA TO TAKE ON";
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
            rewardName.text = level.shipReward.name.ToUpper();
            rewardDescription.text = "UNLOCKS A NEW SHIP TO USE IN RUNS";
            if (unlocked) rewardIcon.color = lightColor;
            else rewardIcon.color = Color.gray;
        }
        else if (level.synergyReward != null)
        {
            if (unlocked)
            {
                lightColor = level.synergyReward.outputCard.color;
                darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);
            }

            rewardIcon.sprite = level.synergyReward.outputCard.sprite;
            rewardName.text = level.synergyReward.name.ToUpper();
            rewardDescription.text = "UNLOCKS A NEW SYNERGY TO CREATE IN RUNS";
            if (unlocked) rewardIcon.color = lightColor;
            else rewardIcon.color = Color.gray;
        }
        else
        {
            if (unlocked)
            {
                lightColor = level.levelColor;
                darkColor = new Color(lightColor.r * 0.2f, lightColor.g * 0.2f, lightColor.b * 0.2f);
                rewardIcon.sprite = level.rewardIcon;
            }
            else rewardIcon.sprite = level.lockedIcon;

            rewardName.text = level.rewardName.ToUpper();
            rewardDescription.text = level.rewardDesc.ToUpper();
            if (unlocked) rewardIcon.color = level.rewardColor;
            else rewardIcon.color = Color.gray;
        }

        // Set colors
        this.levelIcon.color = lightColor;
        border.color = lightColor;
        background.color = darkColor;
        xpAmount.color = darkColor;
        xpBackground.color = lightColor;
        rewardDescription.color = lightColor;
    }
}
