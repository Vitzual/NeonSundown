using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    private AchievementData data;
    public Image icon, iconBorder, border, background, indent;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc, status;

    public AchievementData Data => data;

    public void Setup(AchievementData achievement)
    {
        data = achievement;

        name.text = achievement.obj.Name.ToUpper();
        desc.text = achievement.obj.Description.ToUpper();

        if (achievement.obj.IsAchieved)
        {
            name.color = Color.white;
            desc.color = Color.white;
            icon.sprite = achievement.unlockedIcon;
            border.color = achievement.color;
            iconBorder.color = achievement.color;
            indent.color = achievement.color;
            background.color = new Color(achievement.color.r * 0.2f, 
                achievement.color.g * 0.2f, achievement.color.b * 0.2f, 1);
            status.color = background.color;
            status.text = "UNLOCKED";
        }
        else
        {
            icon.sprite = achievement.lockedIcon;
            status.text = "LOCKED";
        }
    }
}
