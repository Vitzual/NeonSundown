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
    public static Dictionary<Stat, float> moduleEffects;
    public static bool isAlphaBuild;

    // Setup the game
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
        if (arena == null) arena = _arena;
    }

    // On start pass data
    public void Start()
    {
        // Check if save data exists
        if (SaveSystem.saveData == null)
            SaveSystem.GetSave();

        // On ship destroyed, update save
        Events.active.onShipDestroyed += UpdateSave;

        // Set player
        Events.active.SetupShip(ship);
    }

    // Load menu
    public void LoadMenu()
    {
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

        // Check rewards
        if (time >= arena.objectiveOne.timeRequired)
            RewardObjective(arena.objectiveOne);
        if (time >= arena.objectiveTwo.timeRequired)
            RewardObjective(arena.objectiveTwo);
        if (time >= arena.objectiveThree.timeRequired)
            RewardObjective(arena.objectiveThree);
        if (time >= arena.objectiveFour.timeRequired)
            RewardObjective(arena.objectiveFour);

        // Save the game
        SaveSystem.UpdateArena(arena.InternalID, time);
    }

    // Reward objective
    public void RewardObjective(ArenaData.ArenaObjective arenaObjective)
    {
        if (arenaObjective.arenaReward != null)
            SaveSystem.AddArenaUnlock(arenaObjective.arenaReward.InternalID);
        if (arenaObjective.shipReward != null)
            SaveSystem.AddShipUnlock(arenaObjective.shipReward.InternalID);
        if (arenaObjective.cardReward != null)
            SaveSystem.AddCardUnlock(arenaObjective.cardReward.InternalID);
        if (arenaObjective.achievementReward != null)
            SaveSystem.UnlockAchievement(arenaObjective.achievementReward);
    }
}
