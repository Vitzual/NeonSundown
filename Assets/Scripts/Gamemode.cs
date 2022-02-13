using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemode : MonoBehaviour
{
    // Static arena being used
    public static ArenaData arena;
    public static ShipData ship;

    // Setup the game
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
    }

    // On start pass data
    public void Start()
    {
        // Set stages
        if (EnemySpawner.active != null)
            EnemySpawner.active.stages = arena.stages;

        // Set player
        Events.active.SetupShip(ship);
    }

    // Load menu
    public void LoadMenu()
    {
        Dealer.isOpen = false;
        SceneManager.LoadScene("Menu");
    }
}
