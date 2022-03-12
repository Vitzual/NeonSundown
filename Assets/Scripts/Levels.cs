using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public static List<LevelData> ranks;
    public static int level;
    private static bool generated = false;

    // Generate ranks on startup
    public void Awake() 
    {
        // Get a list of all ranks
        if (generated) return;
        ranks = Resources.LoadAll("Levels", typeof(LevelData)).Cast<LevelData>().ToList();
        Debug.Log("Loaded " + ranks.Count + " levels from resource folder");
        generated = true;
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
            LevelData rank = ranks[level];

            // Give card reward
            if (rank.cardReward != null)
                SaveSystem.AddCardUnlock(rank.cardReward.InternalID);

            // Give card reward
            if (rank.arenaReward != null)
                SaveSystem.AddArenaUnlock(rank.arenaReward.InternalID);

            // Give card reward
            if (rank.crystalReward && Scriptables.crystals != null)
                foreach (CrystalData crystal in Scriptables.crystals)
                    SaveSystem.AddCrystal(crystal.InternalID, 5);

            // Increase level
            SaveSystem.LevelUp();
            SaveSystem.UpdateSave();
            level += 1;
        }
    }
}
