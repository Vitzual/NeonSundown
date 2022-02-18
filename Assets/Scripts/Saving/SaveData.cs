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
        crystals = new SerializableDictionary<string, int>();
        modules = new SerializableDictionary<string, int>();
    } 

    public List<string> shipsUnlocked;
    public SerializableDictionary<string, ArenaSave> arenas;
    public SerializableDictionary<string, int> crystals;
    public SerializableDictionary<string, int> modules;
}
