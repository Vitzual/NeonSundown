using HeathenEngineering.SteamworksIntegration;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementTracker : MonoBehaviour
{
    public SteamSettings steam;
    public ProgressBar progressBar;
    public Achievement achievementObj;
    public Transform achievementList;
    public List<AchievementData> achievements;
    private List<Achievement> achievementObjects;

    public void Start()
    {
        // Update all achievements
        CreateAchievements();
        UpdateAchievements();
    }

    public void CreateAchievements()
    {
        achievementObjects = new List<Achievement>();
        foreach (AchievementData achievement in achievements)
        {
            Achievement newAchievement = Instantiate(achievementObj, achievementList);
            newAchievement.GetComponent<RectTransform>().localScale = Vector3.one;
            newAchievement.Setup(achievement);
            achievementObjects.Add(newAchievement);
        }
    }
    
    public void UpdateAchievements()
    {
        int achieved = 0, total = 0;
        foreach (Achievement achievement in achievementObjects)
        {
            achievement.Setup(achievement.Data);
            if (achievement.Data.obj.IsAchieved) 
                achieved += 1;
            total += 1;
        }
        progressBar.maxValue = total;
        progressBar.currentPercent = achieved;
        progressBar.UpdateUI();
    }
}
