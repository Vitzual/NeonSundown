using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public static List<RankData> ranks;
    public static int level;

    // Generate ranks on startup
    public void Awake() 
    {
        // Get a list of all ranks
        ranks = Resources.LoadAll("Ranks", typeof(RankData)).Cast<RankData>().ToList();
        Debug.Log("Loaded " + ranks.Count + " ranks from resource folder");

        foreach (RankData rank in ranks)
            Debug.Log(rank.name);
    }

    // Add XP to the rankup system
    public static void AddXP(int amount)
    {
        // Check if level less then ranks
        if (level < ranks.Count)
        {
            SaveSystem.AddXP(amount);
            if (amount > ranks[level].xpRequirement) LevelUp();
            else SaveSystem.UpdateSave();
        }
    }

    // Level up 
    public static void LevelUp()
    {
        // Check if level less then ranks
        if (level < ranks.Count)
        {
            // Level up and give rewards
            RankData rank = ranks[level];

            // Give card reward
            if (rank.cardReward != null)
                SaveSystem.AddCardUnlock(rank.cardReward.InternalID);

            // Give card reward
            if (rank.arenaReward != null)
                SaveSystem.AddArenaUnlock(rank.arenaReward.InternalID);

            // Give card reward
            if (rank.moduleReward != null)
                SaveSystem.AddModule(rank.moduleReward.InternalID, rank.moduleAmount);

            // Give card reward
            if (rank.crystalReward != null)
                SaveSystem.AddCrystal(rank.crystalReward.InternalID, rank.crystalAmount);

            // Increase level
            SaveSystem.LevelUp();
            SaveSystem.UpdateSave();
            level += 1;
        }
    }
}
