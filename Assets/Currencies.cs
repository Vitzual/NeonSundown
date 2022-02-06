using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Currencies : MonoBehaviour
{
    // Save data
    public static string savePath = "/playerdata.save";

    // Save a game
    public static void SaveGame(int xpCrystals, int bloodCrystals, int lifeCrystals)
    {
        // Grab the persistent data path
        string path = Application.persistentDataPath + savePath;

        // Check if file exists
        if (File.Exists(path))
        {
            // Load json file
            string data = File.ReadAllText(savePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(data);

            // Create new save data instance
            SaveData loadData = new SaveData(saveData.xpCrystals + xpCrystals,
            saveData.bloodCrystals + bloodCrystals, saveData.lifeCrystals + lifeCrystals);

            // Convert to json and save
            string newData = JsonUtility.ToJson(loadData);
            File.WriteAllText(Application.persistentDataPath + savePath, newData);

            // Game saved 
            Debug.Log("[SAVE] Game was saved successfully!");
        }

        // If not, create new file
        else
        {
            // Create new save data instance
            SaveData newSaveData = new SaveData(xpCrystals, bloodCrystals, lifeCrystals);

            // Convert to json and save
            string newData = JsonUtility.ToJson(newSaveData);
            File.WriteAllText(Application.persistentDataPath + savePath, newData);

            // Game saved 
            Debug.Log("[SAVE] Game was saved successfully with new file!");
        }
    }
}