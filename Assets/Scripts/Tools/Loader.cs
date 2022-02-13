using System.IO;
using UnityEngine;

public class Loader
{
    // Most up-to-date data
    public static SaveData saveData;

    // Save path
    protected static string savePath = "/player.save";

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
        File.WriteAllText(Application.persistentDataPath + savePath, newData);

        // Game saved 
        Debug.Log("[SAVE] Game was saved successfully!");
    }

    // Generates the player data
    public static void GetSave()
    {
        // Grab the persistent data path
        string path = Application.persistentDataPath + savePath;

        // Check if file exists
        if (File.Exists(path))
        {
            // Debug save to log
            Debug.Log("[SAVE] Found save data, loading...");

            // Load json file
            string data = File.ReadAllText(savePath);
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
        File.WriteAllText(Application.persistentDataPath + savePath, newData);

        // Game saved 
        Debug.Log("[SAVE] New save data was created successfully!");
    }

    // Checks if a ship is unlocked
    public static bool IsShipUnlocked(string id) { return saveData != null && saveData.shipsUnlocked.Contains(id); }
    public static bool IsArenaUnlocked(string id) { return saveData != null && saveData.arenasUnlocked.Contains(id); }
}
