using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public static List<LevelData> ranks;
    private static bool generated = false;

    // Generate ranks on startup
    public void Awake() 
    {
        // Get a list of all ranks
        if (generated) return;
        ranks = Resources.LoadAll("Levels", typeof(LevelData)).Cast<LevelData>().ToList();
        Debug.Log("Loaded " + ranks.Count + " levels from resource folder");

        // Create sorted list
        LevelData[] sortedList = new LevelData[ranks.Count];
        foreach (LevelData rank in ranks)
            sortedList[int.Parse(rank.name) - 1] = rank;

        // Set ranks to sorted list
        ranks = sortedList.ToList();
        generated = true;
    }

    // Add XP to the rankup system
    public static void AddXP(float amount)
    {
        // Check if level less then ranks
        if (SaveSystem.saveData.level < ranks.Count)
        {
            SaveSystem.saveData.xp += amount;
            if (SaveSystem.saveData.xp > ranks[SaveSystem.saveData.level].xpRequirement) LevelUp();
        }
    }

    // Level up 
    public static void LevelUp()
    {
        // Check if level less then ranks
        if (SaveSystem.saveData.level < ranks.Count)
        {
            // Level up and give rewards
            LevelData rank = ranks[SaveSystem.saveData.level];
            Events.active.LevelUp(rank, SaveSystem.saveData.level);

            // Give card reward
            if (rank.cardReward != null)
                SaveSystem.AddCardUnlock(rank.cardReward.InternalID);

            // Give card reward
            else if (rank.arenaReward != null)
                SaveSystem.AddArenaUnlock(rank.arenaReward.InternalID);

            // Give synergy reward
            else if(rank.synergyReward != null)
                SaveSystem.AddSynergyUnlock(rank.synergyReward.InternalID);

            // Give card reward
            else if(rank.crystalReward && Scriptables.crystals != null)
                foreach (CrystalData crystal in Scriptables.crystals)
                    SaveSystem.AddCrystal(crystal.InternalID, 5);

            // Give reroll reward
            else if(rank.redrawReward)
                SaveSystem.AddRedraws(1);

            // Increase level
            SaveSystem.saveData.xp -= rank.xpRequirement;
            SaveSystem.saveData.level += 1;
            if (XPHandler.active != null) 
                XPHandler.active.UpdateRewards();
            SaveSystem.UpdateSave();
        }
    }
}
