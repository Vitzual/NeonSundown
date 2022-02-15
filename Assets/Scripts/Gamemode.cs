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
        // Get objective status
        bool primary = CheckObjective(arena.primaryObjective);
        bool secondary = CheckObjective(arena.secondaryObjective);

        // If objective complete, award them
        if (primary) RewardObjective(arena.primaryObjective);
        if (secondary) RewardObjective(arena.secondaryObjective);

        // Save the game
        SaveSystem.UpdateArena(arena.InternalID, EnemySpawner.GetTime(), primary, secondary);
    }

    // Check objectives
    public bool CheckObjective(ArenaData.ArenaObjective arenaObjective)
    {
        switch(arenaObjective.objective)
        {
            case Objective.Survival:
                return EnemySpawner.GetTime() >= arenaObjective.surviveTime;

            case Objective.Extermination:
                break;

            case Objective.Guardian:
                break;

            default:
                break;

                
        }

        return false;
    }

    // Reward objective
    public void RewardObjective(ArenaData.ArenaObjective arenaObjective)
    {
        if (arenaObjective.shipReward != null)
            SaveSystem.AddShipUnlock(arenaObjective.shipReward.InternalID);
    }
}
