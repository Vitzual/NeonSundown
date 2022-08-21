using System;
using System.Collections.Generic;
using UnityEngine;

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
        audioModsUnlocked = new List<string>();
        blackmarketItemsPurchased = new List<string>();
        arenaTimes = new SerializableDictionary<string, float>();
        crystals = new SerializableDictionary<string, int>();
        modules = new SerializableDictionary<string, int>();
        discordReward = false;
    }

    public long epochMillisecond;
    public float xp;
    public int redraws, level;
    public List<string> arenasUnlocked;
    public List<string> shipsUnlocked;
    public List<string> cardsUnlocked;
    public List<string> synergiesUnlocked;
    public List<string> audioModsUnlocked;
    public List<string> blackmarketItemsPurchased;
    public SerializableDictionary<string, float> arenaTimes;
    public SerializableDictionary<string, int> crystals;
    public SerializableDictionary<string, int> modules;
    public bool discordReward = false;
}
