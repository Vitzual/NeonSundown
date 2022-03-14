using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public static List<LevelData> ranks;
    private static bool generated = false;

    // Generate ranks on startup
    public void Awake() { GenerateRanks(); }

    // Generate ranks
    private static void GenerateRanks()
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

    // Update levels on load
    public static void UpdateUnlocks()
    {
        if (ranks == null)
            GenerateRanks();

        foreach (LevelData level in ranks)
        {
            if (level.arenaReward != null && !SaveSystem.IsArenaUnlocked(level.arenaReward.InternalID))
                SaveSystem.AddArenaUnlock(level.arenaReward.InternalID);
            else if (level.shipReward != null && !SaveSystem.IsShipUnlocked(level.shipReward.InternalID))
                SaveSystem.AddShipUnlock(level.shipReward.InternalID);
            else if(level.cardReward != null && !SaveSystem.IsCardUnlocked(level.cardReward.InternalID))
                SaveSystem.AddCardUnlock(level.cardReward.InternalID);
            else if(level.synergyReward != null && !SaveSystem.IsSynergyUnlocked(level.synergyReward.InternalID))
                SaveSystem.AddSynergyUnlock(level.synergyReward.InternalID);
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

            // Give card reward
            else if (rank.shipReward != null)
                SaveSystem.AddShipUnlock(rank.shipReward.InternalID);

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
