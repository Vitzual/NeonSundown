using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public static class Scriptables
{
    // Resource paths
    public static string CardsPath = "Cards";
    public static string EnemiesPath = "Enemies";
    public static string StagesPath = "Stages";

    // Scriptable dictionaries
    public static Dictionary<string, CardData> cardsDict;
    public static Dictionary<string, EnemyData> enemiesDict;
    public static Dictionary<string, StageData> stagesDict;

    // Scriptable lists
    public static List<CardData> cards;
    public static List<EnemyData> enemies;
    public static List<StageData> stages;

    // Generate scriptables
    public static void GenerateAllScriptables()
    {
        GenerateCards();
        GenerateEnemies();
        GenerateStages();
    }

    // Generate buildings on startup
    public static void GenerateCards()
    {
        cardsDict = new Dictionary<string, CardData>();
        cards = new List<CardData>();

        List<CardData> loaded = Resources.LoadAll(CardsPath, typeof(CardData)).Cast<CardData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " cards from " + CardsPath);

        foreach (CardData card in loaded)
        {
            cardsDict.Add(card.InternalID, card);
            cards.Add(card);
            Debug.Log("Loaded " + card.name + " with UUID " + card.InternalID);
        }
    }

    // Generate buildings on startup
    public static void GenerateEnemies()
    {
        enemiesDict = new Dictionary<string, EnemyData>();
        enemies = new List<EnemyData>();

        List<EnemyData> loaded = Resources.LoadAll(EnemiesPath, typeof(EnemyData)).Cast<EnemyData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " enemies from " + EnemiesPath);

        foreach (EnemyData enemy in loaded)
        {
            enemy.GenerateStats();
            enemiesDict.Add(enemy.InternalID, enemy);
            enemies.Add(enemy);
            Debug.Log("Loaded " + enemy.name + " with UUID " + enemy.InternalID);
        }
    }

    // Generate buildings on startup
    public static void GenerateStages()
    {
        stagesDict = new Dictionary<string, StageData>();
        stages = new List<StageData>();

        List<StageData> loaded = Resources.LoadAll(StagesPath, typeof(StageData)).Cast<StageData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " stages from " + StagesPath);

        foreach (StageData stage in loaded)
        {
            stagesDict.Add(stage.InternalID, stage);
            stages.Add(stage);
            Debug.Log("Loaded " + stage.name + " with UUID " + stage.InternalID);
        }
    }
}