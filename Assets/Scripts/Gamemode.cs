using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemode : MonoBehaviour
{
    // Static arena being used
    public static ArenaData arena;

    // Setup the game
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
    }

    // Load menu
    public void LoadMenu()
    {
        Dealer.isOpen = false;
        SceneManager.LoadScene("Menu");
    }
}
