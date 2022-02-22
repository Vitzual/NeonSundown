using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    // Constructor
    public SaveData()
    {
        arenasUnlocked = new List<string>();
        shipsUnlocked = new List<string>();
        cardsUnlocked = new List<string>();
        arenaTimes = new SerializableDictionary<string, float>();
        crystals = new SerializableDictionary<string, int>();
        modules = new SerializableDictionary<string, int>();
    }

    public List<string> arenasUnlocked;
    public List<string> shipsUnlocked;
    public List<string> cardsUnlocked;
    public SerializableDictionary<string, float> arenaTimes;
    public SerializableDictionary<string, int> crystals;
    public SerializableDictionary<string, int> modules;
}
