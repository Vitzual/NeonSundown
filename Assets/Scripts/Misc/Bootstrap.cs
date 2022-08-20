using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    // Canvas group
    public CanvasGroup canvasGroup, saveGroup;
    private float cooldown = 0.5f;
    private bool startCooldown = false;

    // Save paths
    private const string OLD_SAVE_PATH = "/player_save.json";
    private const string NEW_SAVE_PATH = "/V2_player_save.json";

    
    // Start is called before the first frame update
    public void Start()
    {
        // Get all save paths
        string old_save = Application.persistentDataPath + OLD_SAVE_PATH;
        string new_save = Application.persistentDataPath + NEW_SAVE_PATH;

        // Check for experimental save
        if (File.Exists(new_save))
        {
            // Debug to log
            Debug.Log("[SAVE] Save file detected.");

            if (File.Exists(old_save))
            {
                // Debug to log
                Debug.Log("[SAVE] Old save file detected.");

                // Load json file
                string data = File.ReadAllText(new_save);
                SaveData newSaveData = JsonUtility.FromJson<SaveData>(data);

                // Check if 
                if (newSaveData.oldSave)
                {
                    Debug.Log("[SAVE] New save not updated, prompting for save selection.");
                    LeanTween.alphaCanvas(saveGroup, 1f, 1f);
                    saveGroup.interactable = true;
                    return;
                }
                else Debug.Log("[SAVE] Ignoring old save file.");
            }

            // Load main
            LeanTween.alphaCanvas(canvasGroup, 1f, 1f);
            LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setDelay(6f);
            startCooldown = true;
        }
        else if (File.Exists(old_save))
        {
            // Debug to log
            Debug.Log("[SAVE] An old save file was found, but no new save file was found. Generating new V2 save.");

            // Load json file
            string data = File.ReadAllText(old_save);
            SaveData oldSaveData = JsonUtility.FromJson<SaveData>(data);

            // Write to file
            SaveSystem.saveData = oldSaveData;
            SaveSystem.UpdateSave();

            // Load main
            LeanTween.alphaCanvas(canvasGroup, 1f, 1f);
            LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setDelay(6f);
            startCooldown = true;
        }
        else
        {
            // Debug to log
            Debug.Log("[SAVE] No save files were found.");

            LeanTween.alphaCanvas(canvasGroup, 1f, 1f);
            LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setDelay(6f);
            startCooldown = true;
        }
    }
    
    public void ChooseSave(bool old)
    {
        saveGroup.interactable = false;

        if (old)
        {
            Debug.Log("[SAVE] Old save selected, replacing experimental data with V1 data.");
            string old_save = Application.persistentDataPath + OLD_SAVE_PATH;
            string data = File.ReadAllText(old_save);
            SaveData newSaveData = JsonUtility.FromJson<SaveData>(data);
            newSaveData.oldSave = false;
            SaveSystem.saveData = newSaveData;
            SaveSystem.UpdateSave();
        }
        else
        {
            Debug.Log("[SAVE] New save selected, setting updated flag to true.");
            SaveSystem.GetSave();
            SaveSystem.saveData.oldSave = false;
            SaveSystem.UpdateSave();
        }

        LeanTween.alphaCanvas(saveGroup, 0f, 1f);
        LeanTween.alphaCanvas(canvasGroup, 1f, 1f).setDelay(1f);
        LeanTween.alphaCanvas(canvasGroup, 0f, 1f).setDelay(7f);
        cooldown = 1.5f;
        startCooldown = true;
    }

    // Update method
    public void Update()
    {
        if (startCooldown)
        {
            if (cooldown > 0) cooldown -= Time.deltaTime;
            else if (canvasGroup.alpha == 0f) SceneManager.LoadScene("Menu");
        }
    }
}
