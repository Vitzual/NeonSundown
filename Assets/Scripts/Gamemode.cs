using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemode : MonoBehaviour
{
    // Scene reference for testing
    public ArenaData _arena;

    // Static arena being used
    public static ArenaData arena;
    public static ShipData ship;
    public static Dictionary<int, ModuleData> modules;
    public static bool isAlphaBuild;

    // Setup the game
    public void Awake()
    {
        RuntimeStats.ResetStats();
        Scriptables.GenerateAllScriptables();
        if (arena == null) arena = _arena;
    }

    // On start pass data
    public void Start()
    {
        // Check if save data exists
        if (SaveSystem.saveData == null)
            SaveSystem.GetSave();
    }

    // Load menu
    public void LoadMenu()
    {
        UpdateSave();
        Dealer.isOpen = false;
        SceneManager.LoadScene("Menu");
    }

    // Update arena
    public void UpdateSave()
    {
        // Check if build is alpha
        if (isAlphaBuild) return;

        // Get arena time
        float time = EnemySpawner.GetTime();

        // Save the game
        SaveSystem.UpdateArena(arena.InternalID, time);
        SaveSystem.UpdateSave();
    }
}
