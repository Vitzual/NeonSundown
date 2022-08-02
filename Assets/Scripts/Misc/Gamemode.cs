using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gamemode : MonoBehaviour
{
    // Scene reference for testing
    public ArenaData _arena;

    // Static arena being used
    public Ship ship;
    public static ArenaData arena;
    public static ShipData shipData;
    public static Dictionary<int, ModuleData> modules;
    public static List<CardData> startingCards;
    public static List<CardData> blacklistCards;
    public static bool isAlphaBuild;

    // Arena interface variables
    public GameObject arenaInfo;
    public Image arenaIcon;
    public TextMeshProUGUI arenaName, arenaDesc;
    public bool useArenaInfo = false;
    private static bool awardGiven = false;

    // Enemies objective tracking
    public static bool killEnemiesObjective;
    public static EnemyData enemyObjective;
    private static int objectiveAmountKilled = 0;

    // Arena specific modifiers
    public Vault vault;
    public Vector2 vaultPosition;
    public Vector2 vaultPlayerPosition;

    // Reference to in-game camera
    public new Camera camera;

    // Setup the game
    public void Awake()
    {
        RuntimeStats.ResetStats();
        Scriptables.GenerateAllScriptables();
        if (arena == null) arena = _arena;
    }

    // On start pass data
    public void Start()
    {
        // Add starting cards
        if (startingCards == null) startingCards = new List<CardData>();
        else foreach (CardData card in startingCards) Deck.active.AddCard(card);

        // Add blacklist cards
        if (blacklistCards == null) blacklistCards = new List<CardData>();
        else foreach (CardData card in arena.blacklistCards) blacklistCards.Add(card);

        // Check if save data exists
        if (SaveSystem.saveData == null)
            SaveSystem.GetSave();

        // Check if use arena info
        if (arena.showObjectiveOnStart)
        {
            arenaIcon.sprite = arena.unlockedIcon;
            arenaName.text = arena.name.ToUpper();
            arenaDesc.text = arena.achievementObjective.ToUpper();
            arenaDesc.color = arena.lightColor;
            LeanTween.moveLocal(arenaInfo, new Vector2(0, -50), 1.5f).setEase(LeanTweenType.easeOutExpo).setDelay(1f);
            LeanTween.moveLocal(arenaInfo, new Vector2(0, 50), 0.5f).setEase(LeanTweenType.easeInExpo).setDelay(4f);
        }

        // Check if use vault
        if (arena.useVault)
        {
            Vault newVault = Instantiate(vault, vaultPosition, Quaternion.identity);
            ship.transform.position = vaultPlayerPosition;
        }

        // Set camera viewing range
        if (camera != null) camera.orthographicSize = arena.startingViewRange;
    }

    // Load menu
    public void LoadMenu()
    {
        UpdateSave();
        Dealer.isOpen = false;
        SceneManager.LoadScene("Menu");
    }

    // Update arena
    public void UpdateSave()
    {
        // Check if build is alpha
        if (isAlphaBuild) return;

        // Get arena time
        float time = ArenaController.GetTime();

        // Save the game
        SaveSystem.UpdateArena(arena.InternalID, time);
        SaveSystem.UpdateSave();
    }

    // Defeat boss
    public static void ObjectiveEnemyKilled()
    {
        objectiveAmountKilled += 1;
        if (objectiveAmountKilled > arena.amountToKill && !awardGiven)
        {
            awardGiven = true;
            if (!arena.achievement.IsAchieved)
            {
                arena.achievement.Unlock();
                arena.achievement.Store();
            }
            else Debug.Log("Achievement has already been given!");
        }
    }
}
