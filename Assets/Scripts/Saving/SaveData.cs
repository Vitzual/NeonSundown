using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    // Constructor
    public SaveData()
    {
        shipsUnlocked = new List<string>();
        arenas = new SerializableDictionary<string, ArenaSave>();
    } 

    public List<string> shipsUnlocked;
    public SerializableDictionary<string, ArenaSave> arenas;
}
