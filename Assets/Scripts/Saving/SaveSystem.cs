using System.IO;
using UnityEngine;

public class SaveSystem
{
    // Save path
    private const string SAVE_PATH = "/player.save";

    // Most up-to-date data
    public static SaveData saveData;

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
            // Tells system if it should update save
            bool updateSave = false;

            // Check if best time achieved
            if (saveData.arenaTimes[id] < time)
            {
                saveData.arenaTimes[id] = time;
                updateSave = true;
            }

            // Update save if requried
            if (updateSave) UpdateSave();
        }

        // If arena does not exist, create new instance
        else
        {
            saveData.arenaTimes.Add(id, time);
            Debug.Log("Created arena " + id);
            UpdateSave();
        }
    }

    // Saves arena time
    public static void AddShipUnlock(string id)
    {
        // Update the arena time
        if (!saveData.shipsUnlocked.Contains(id))
        {
            saveData.shipsUnlocked.Add(id);
            UpdateSave();
        }
    }

    // Checks if a ship is unlocked
    public static bool IsArenaUnlocked(string id) { return saveData != null && saveData.arenasUnlocked.Contains(id); }
    public static bool IsShipUnlocked(string id) { return saveData != null && saveData.shipsUnlocked.Contains(id); }
    public static bool IsCardUnlocked(string id) { return saveData != null && saveData.cardsUnlocked.Contains(id); }
}
