using CloudAPI = HeathenEngineering.SteamworksIntegration.API.RemoteStorage.Client;
using HeathenEngineering.SteamworksIntegration;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HeathenEngineering.SteamworksIntegration.API;
using System.Linq;

public class SaveSystem
{
    // Save path
    private const string CLOUD_SAVE_NAME = "player_save.json";
    private const string SAVE_PATH = "/player_save.json";
    private const string META_PATH = "/context_save.json";
    private const int MAX_ARENA_TIMES = 10;

    // Most up-to-date data
    public static SaveData saveData;

    // Sets the meta context
    public static void SetMetacontext(string arena, string ship, List<string> modules)
    {
        // Log to the thing
        Debug.Log("[SAVE] Setting meta context...");

        // Create new meta context
        MetaContext context = new MetaContext(arena, ship, modules);

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

        // Try to write to cloud
        try
        {
            if (CloudAPI.IsEnabled)
            {
                CloudAPI.FileWriteAsync(CLOUD_SAVE_NAME, newData, System.Text.Encoding.UTF8, (result, hasError) =>
                {
                    if (!hasError) Debug.Log("[CLOUD SAVE] Game was successfully saved on Steam cloud!");
                    else Debug.Log("[CLOUD SAVE] Game could not be saved on Steam cloud!");
                });
            }
        }
        catch { Debug.Log("[CLOUD] Ran into error while trying to access Steam cloud."); }

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

        // Try to write to cloud
        try
        {
            if (CloudAPI.IsEnabled)
            {
                // Check cloud storage
                CloudAPI.GetQuota(out ulong total, out ulong remaining);
                Debug.Log("[CLOUD] Used " + (total - remaining) + " of " + total + " bytes on Steam cloud.");
                RemoteStorageFile[] files = CloudAPI.GetFiles();
                Debug.Log("[CLOUD] Retrieved " + files.Length + " from Steam cloud.");

                // Attempt to get the object from the cloud
                SaveData cloudSave = CloudAPI.FileReadJson<SaveData>(CLOUD_SAVE_NAME, System.Text.Encoding.UTF8);
                Debug.Log("[CLOUD] Attempting to load save file from cloud");

                // If save is not null, compare timestamps
                if (cloudSave != null && saveData != null)
                {
                    if (cloudSave.epochMillisecond > saveData.epochMillisecond)
                    {
                        Debug.Log("[CLOUD] Found newer cloud save, overwriting local save.");
                        saveData = cloudSave;
                    }
                }
                else if (cloudSave != null && saveData == null)
                {
                    Debug.Log("[CLOUD] No local save data found, using cloud save.");
                    saveData = cloudSave;
                }
            }
        }
        catch { Debug.Log("[CLOUD] Ran into error while trying to access Steam cloud."); }

        // If save data not loaded, generate new save
        if (saveData == null) GenerateSave();
    }

    // Check if save exists
    public static bool HasSave()
    {
        // Grab the persistent data path
        string path = Application.persistentDataPath + META_PATH;

        // Check if file exists
        return File.Exists(path);
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

        // Try to write to cloud
        try 
        { 
            if (CloudAPI.IsEnabled)
            {
                CloudAPI.FileWriteAsync(CLOUD_SAVE_NAME, newData, System.Text.Encoding.UTF8, (result, hasError) =>
                {
                    if (!hasError) Debug.Log("[CLOUD SAVE] New save data was created successfully on Steam cloud!");
                    else Debug.Log("[CLOUD SAVE] New save data could not be written to Steam cloud!");
                });
            }
        } 
        catch { Debug.Log("[CLOUD] Ran into error while trying to access Steam cloud."); }

        // Game saved 
        Debug.Log("[SAVE] New save data was created successfully!");
    }

    // Saves arena time
    public static void UpdateArena(string id, float time)
    {
        // Check if gamemode ship null
        if (Gamemode.shipData == null)
        {
            Debug.Log("[WARNING] Gamemode ship was null! Cannot save.");
            return;
        }

        // Update the arena time
        if (saveData.arenaTimes.ContainsKey(id))
        {
            /* Keep track of total times
            int totalTimes = 0;

            // Check if best time achieved
            for (int i = 0; i < saveData.newArenaTimes[id].Length; i++)
            {
                // Check if total times over max amount
                if (saveData.newArenaTimes[id][i] != null && 
                    saveData.newArenaTimes[id][i].time < time)
                {
                    TimeData holder = saveData.newArenaTimes[id][i];
                    saveData.newArenaTimes[id][i] = newData;
                    newData = holder;
                }

                // Increase total times
                totalTimes += 1;
                if (totalTimes >= MAX_ARENA_TIMES) break;
            }

            // Check if total times reached max, and if not create new best time
            if (totalTimes < MAX_ARENA_TIMES) saveData.newArenaTimes[id][totalTimes] = newData;
            */
        }

        // If arena does not exist, create new instance
        else
        {
            
            Debug.Log("Created arena " + id);
        }
    }

    // Add a crystal to save
    public static void AddXP(float amount) { saveData.xp += amount; }
    public static void LevelUp() { saveData.level += 1; }

    // Add a crystal to save
    public static void AddModule(string id)
    {
        if (saveData.modules.ContainsKey(id))
            saveData.modules[id] += 1;
        else saveData.modules.Add(id, 0);
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
    public static void AddSynergyUnlock(string id)
    {
        // Update the card unlocks
        if (!saveData.synergiesUnlocked.Contains(id))
            saveData.synergiesUnlocked.Add(id);
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

    // Get best time
    public static void AddRedraws(int amount)
    {
        if (saveData != null)
            saveData.redraws += amount;
    }

    // Get best time
    public static int GetRedraws()
    {
        if (saveData != null)
            return saveData.redraws;
        else return 1;
    }

    // Check if user has module
    public static bool HasModule(string id)
    {
        return saveData.modules.ContainsKey(id);
    }

    // Check if user has crystal
    public static int GetCrystalAmount(string id)
    {
        if (saveData.crystals.ContainsKey(id))
            return saveData.crystals[id];
        else return 0;
    }

    // Check if user has crystal
    public static int GetModuleAmount(string id)
    {
        if (saveData.modules.ContainsKey(id))
        {
            if (saveData.modules[id] > 4) return 4;
            else return saveData.modules[id];
        }
        else return -1;
    }

    // Returns the players level
    public static int GetPlayerLevel() 
    {
        if (saveData != null)
            return saveData.level;
        else return 1;
    }
    
    // Checks if a ship is unlocked
    public static bool IsArenaUnlocked(string id) { return saveData != null && saveData.arenasUnlocked.Contains(id); }
    public static bool IsShipUnlocked(string id) { return saveData != null && saveData.shipsUnlocked.Contains(id); }
    public static bool IsCardUnlocked(string id) { return saveData != null && saveData.cardsUnlocked.Contains(id); }
    public static bool IsSynergyUnlocked(string id) { return saveData != null && saveData.synergiesUnlocked.Contains(id); }
    public static bool IsPlayerMaxLevel() { return saveData != null && saveData.level >= Levels.ranks.Count; }
}
