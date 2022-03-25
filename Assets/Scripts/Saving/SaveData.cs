using System;
using System.Collections.Generic;

public class SaveData
{
    // Constructor
    public SaveData()
    {
        // Get epoch second
        DateTimeOffset now = DateTimeOffset.UtcNow;
        epochMillisecond = now.ToUnixTimeMilliseconds();

        xp = 0;
        level = 0;
        redraws = 1;
        arenasUnlocked = new List<string>();
        shipsUnlocked = new List<string>();
        cardsUnlocked = new List<string>();
        synergiesUnlocked = new List<string>();
        arenaTimes = new SerializableDictionary<string, float>();
        newArenaTimes = new SerializableDictionary<string, List<TimeData>>();
        crystals = new SerializableDictionary<string, int>();
        modules = new SerializableDictionary<string, int>();
        tempArenaTimeConverted = true;
    }

    public long epochMillisecond;
    public float xp;
    public int redraws, level;
    public List<string> arenasUnlocked;
    public List<string> shipsUnlocked;
    public List<string> cardsUnlocked;
    public List<string> synergiesUnlocked;
    public SerializableDictionary<string, float> arenaTimes;
    public SerializableDictionary<string, List<TimeData>> newArenaTimes;
    public SerializableDictionary<string, int> crystals;
    public SerializableDictionary<string, int> modules;

    public bool tempArenaTimeConverted = false;
}
