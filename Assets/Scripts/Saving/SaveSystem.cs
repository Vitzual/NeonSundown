using HeathenEngineering.SteamworksIntegration;
using HeathenEngineering.SteamworksIntegration.API;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    // Save path
    private const string SAVE_PATH = "/player.json";
    private const string META_PATH = "/context.json";

    // Most up-to-date data
    public static SaveData saveData;

    // Sets the meta context
    public static void SetMetacontext(string arena, string ship, List<string> blacklist)
    {
        // Log to the thing
        Debug.Log("[SAVE] Setting meta context...");

        // Create new meta context
        MetaContext context = new MetaContext(arena, ship, blacklist);

        // Overwrite previous context
        string newData = JsonUtility.ToJson(context);
        File.WriteAllText(Application.persistentDataPath + META_PATH, newData);

        // Log to the thing
        Debug.Log("[SAVE] Context set successfully! Starting game...");
    }

    // Get the meta context
    public static MetaContext GetMetacontext()
    {
        // Grab the persistent data path
        string path = Application.persistentDataPath + META_PATH;

        // Check if file exists
        if (File.Exists(path))
        {
            // Debug save to log
            Debug.Log("[SAVE] Found context data, loading...");

            // Load json file
            string data = File.ReadAllText(path);
            return JsonUtility.FromJson<MetaContext>(data);
        }
        else return null;
    }

    // Update the save file
    public static void UpdateSave()
    {
        // Check if save exists
        if (saveData == null)
        {
            GenerateSave();
            return;
        }

        // Debug new save to log
        Debug.Log("[SAVE] Attempting to update save...");

        // Convert to json and save
        string newData = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + SAVE_PATH, newData);

        // Game saved 
        Debug.Log("[SAVE] Game was saved successfully!");
    }

    // Generates the player data
    public static void GetSave()
    {
        // Grab the persistent data path
        string path = Application.persistentDataPath + SAVE_PATH;

        // Check if file exists
        if (File.Exists(path))
        {
            // Debug save to log
            Debug.Log("[SAVE] Found save data, loading...");

            // Load json file
            string data = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(data);

            // Game saved 
            Debug.Log("[SAVE] Successfully loaded save data!");
        }

        // If file does not exist, generate it
        else GenerateSave();
    }

    // Generate a new save
    public static void GenerateSave()
    {
        // Debug new save to log
        Debug.Log("[SAVE] Generating a new save file...");

        // Create new save data instance
        saveData = new SaveData();

        // Convert to json and save
        string newData = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + SAVE_PATH, newData);

        // Game saved 
        Debug.Log("[SAVE] New save data was created successfully!");
    }

    // Saves arena time
    public static void UpdateArena(string id, float time)
    {
        // Update the arena time
        if (saveData.arenaTimes.ContainsKey(id))
        {
            // Check if best time achieved
            if (saveData.arenaTimes[id] < time)
                saveData.arenaTimes[id] = time;
        }

        // If arena does not exist, create new instance
        else
        {
            saveData.arenaTimes.Add(id, time);
            Debug.Log("Created arena " + id);
        }
    }

    // Add a crystal to save
    public static void AddXP(int amount) { saveData.xp += amount; }
    public static void LevelUp() { saveData.level += 1; }

    // Add a crystal to save
    public static void AddModule(string id, int amount)
    {
        if (saveData.modules.ContainsKey(id))
            saveData.modules[id] += amount;
        else if (amount > 0) saveData.modules.Add(id, amount);
    }

    // Add a crystal to save
    public static void AddCrystal(string id, int amount)
    {
        if (saveData.crystals.ContainsKey(id))
            saveData.crystals[id] += amount;
        else if (amount > 0) saveData.crystals.Add(id, amount);
    }

    // Saves arena time
    public static void AddArenaUnlock(string id)
    {
        // Update the arena unlocks
        if (!saveData.arenasUnlocked.Contains(id))
            saveData.arenasUnlocked.Add(id);
    }

    // Saves arena time
    public static void AddShipUnlock(string id)
    {
        // Update the ship unlocks
        if (!saveData.shipsUnlocked.Contains(id))
            saveData.shipsUnlocked.Add(id);
    }

    // Saves arena time
    public static void AddCardUnlock(string id)
    {
        // Update the card unlocks
        if (!saveData.cardsUnlocked.Contains(id))
            saveData.cardsUnlocked.Add(id);
    }

    // Saves arena time
    public static void UnlockAchievement(AchievementObject achievement)
    {
        // Give the achievement to the user
        if (!achievement.IsAchieved)
        {
            achievement.Unlock();
            achievement.Store();
        }
    }

    // Get best time
    public static float GetBestTime(string id)
    {
        if (saveData != null && saveData.arenaTimes.ContainsKey(id))
            return saveData.arenaTimes[id];
        else return 0;
    }

    // Check if user has module
    public static bool HasModule(string id)
    {
        return saveData.modules.ContainsKey(id) && saveData.modules[id] > 0;
    }

    // Check if user has crystal
    public static bool HasCrystal(string id)
    {
        return saveData.crystals.ContainsKey(id) && saveData.crystals[id] > 0;
    }

    // Checks if a ship is unlocked
    public static bool IsArenaUnlocked(string id) { return saveData != null && saveData.arenasUnlocked.Contains(id); }
    public static bool IsShipUnlocked(string id) { return saveData != null && saveData.shipsUnlocked.Contains(id); }
    public static bool IsCardUnlocked(string id) { return saveData != null && saveData.cardsUnlocked.Contains(id); }
}
