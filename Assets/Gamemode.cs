using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemode : MonoBehaviour
{
    // Setup the game
    public void Awake()
    {
        Scriptables.GenerateAllScriptables();
    }
}
