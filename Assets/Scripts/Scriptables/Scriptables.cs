using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Loads scriptables at runtime which can then be accesed from anywhere.

public static class Scriptables
{
    // Flag for calculating arena stage times
    private static bool generated = false;

    // Resource paths
    public static string CardsPath = "Cards";
    public static string EnemiesPath = "Enemies";
    public static string StagesPath = "Stages";
    public static string ArenasPath = "Arenas";
    public static string ShipsPath = "Ships";
    public static string CrystalsPath = "Crystals";
    public static string SynergiesPath = "Synergies";
    public static string ModulesPath = "Modules";
    public static string BlackmarketPath = "Blackmarket";

    // Scriptable dictionaries
    public static Dictionary<string, CardData> cardsDict;
    public static Dictionary<string, EnemyData> enemiesDict;
    public static Dictionary<string, StageData> stagesDict;
    public static Dictionary<string, ArenaData> arenasDict;
    public static Dictionary<string, ShipData> shipsDict;
    public static Dictionary<string, ModuleData> modulesDict;
    public static Dictionary<string, BlackmarketData> blackmarketDict;

    // Scriptable lists
    public static List<CardData> cards;
    public static List<EnemyData> enemies;
    public static List<StageData> stages;
    public static List<ArenaData> arenas;
    public static List<ShipData> ships;
    public static List<CrystalData> crystals;
    public static List<SynergyData> synergies;
    public static List<ModuleData> modules;
    public static List<BlackmarketData> blackmarketItems;

    // Generate scriptables
    public static void GenerateAllScriptables()
    {
        // Check if already generated
        if (generated) return;

        // If not, set them
        GenerateCards();
        GenerateEnemies();
        GenerateStages();
        GenerateArenas();
        GenerateShips();
        GenerateCrystals();
        GenerateSynergies();
        GenerateModules();
        GenerateBlackmarketItems();

        // Set generated to true
        generated = true;
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

    // Generate buildings on startup
    public static void GenerateArenas()
    {
        arenasDict = new Dictionary<string, ArenaData>();
        arenas = new List<ArenaData>();

        List<ArenaData> loaded = Resources.LoadAll(ArenasPath, typeof(ArenaData)).Cast<ArenaData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " arenas from " + ArenasPath);

        foreach (ArenaData arena in loaded)
        {
            arenasDict.Add(arena.InternalID, arena);
            arenas.Add(arena);

            float runningTime = 0;
            foreach(StageData stage in arena.stages)
                runningTime = stage.CalcTotal(runningTime);

            Debug.Log("Loaded " + arena.name + " with UUID " + arena.InternalID);
        }
    }

    // Generate buildings on startup
    public static void GenerateShips()
    {
        shipsDict = new Dictionary<string, ShipData>();
        ships = new List<ShipData>();

        List<ShipData> loaded = Resources.LoadAll(ShipsPath, typeof(ShipData)).Cast<ShipData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " ships from " + ShipsPath);

        foreach (ShipData ship in loaded)
        {
            shipsDict.Add(ship.InternalID, ship);
            ships.Add(ship);
            Debug.Log("Loaded " + ship.name + " with UUID " + ship.InternalID);
        }
    }

    // Generate crystals on startup
    public static void GenerateCrystals()
    {
        crystals = new List<CrystalData>();

        List<CrystalData> loaded = Resources.LoadAll(CrystalsPath, typeof(CrystalData)).Cast<CrystalData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " crystals from " + CrystalsPath);

        foreach (CrystalData crystal in loaded)
        {
            crystals.Add(crystal);
            Debug.Log("Loaded " + crystal.name + " with UUID " + crystal.InternalID);
        }
    }

    // Generate synergies on startup
    public static void GenerateSynergies()
    {
        synergies = new List<SynergyData>();

        List<SynergyData> loaded = Resources.LoadAll(SynergiesPath, typeof(SynergyData)).Cast<SynergyData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " synergies from " + SynergiesPath);

        foreach (SynergyData synergy in loaded)
        {
            synergies.Add(synergy);
            Debug.Log("Loaded " + synergy.name + " with UUID " + synergy.InternalID);
        }
    }

    // Generate modules on startup
    public static void GenerateModules()
    {
        modulesDict = new Dictionary<string, ModuleData>();
        modules = new List<ModuleData>();

        List<ModuleData> loaded = Resources.LoadAll(ModulesPath, typeof(ModuleData)).Cast<ModuleData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " modules from " + ModulesPath);

        foreach (ModuleData module in loaded)
        {
            modules.Add(module);
            modulesDict.Add(module.InternalID, module);
            Debug.Log("Loaded " + module.name + " with UUID " + module.InternalID);
        }
    }

    // Generate blackmarket items on startup
    public static void GenerateBlackmarketItems()
    {
        blackmarketDict = new Dictionary<string, BlackmarketData>();
        blackmarketItems = new List<BlackmarketData>();

        List<BlackmarketData> loaded = Resources.LoadAll(BlackmarketPath, typeof(BlackmarketData)).Cast<BlackmarketData>().ToList();
        Debug.Log("Loaded " + loaded.Count + " blackmarket items from " + BlackmarketPath);

        foreach (BlackmarketData item in loaded)
        {
            blackmarketItems.Add(item);
            blackmarketDict.Add(item.InternalID, item);
            Debug.Log("Loaded " + item.name + " with UUID " + item.InternalID);
        }
    }
}