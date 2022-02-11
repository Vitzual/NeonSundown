using System.IO;
using UnityEngine;

public class Loader
{
    // Save path
    public static string savePath = "/player.save";

    // Retrieves the arena data
    public static PlayerSave GetPlayerSave()
    {
        // Grab the persistent data path
        string path = Application.persistentDataPath + savePath;

        // Check if file exists
        if (File.Exists(path))
        {
            // Load json file
            string data = File.ReadAllText(savePath);
            return JsonUtility.FromJson<PlayerSave>(data);
        }
        else return null;
    }
}
