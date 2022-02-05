using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemode : MonoBehaviour
{
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
