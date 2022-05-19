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
    public static ArenaData arena;
    public static ShipData ship;
    public static Dictionary<int, ModuleData> modules;
    public static bool isAlphaBuild;

    // Arena interface variables
    public GameObject arenaInfo;
    public Image arenaIcon;
    public TextMeshProUGUI arenaName, arenaDesc;
    public bool useArenaInfo = false;
    private bool awardGiven = false;

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
        // Check if save data exists
        if (SaveSystem.saveData == null)
            SaveSystem.GetSave();

        // Check if use arena info
        if (useArenaInfo && arenaIcon != null)
        {
            arenaIcon.sprite = arena.unlockedIcon;
            arenaName.text = arena.name.ToUpper();
            arenaDesc.text = arena.achievementObjective.ToUpper();
            arenaDesc.color = arena.lightColor;
            LeanTween.moveLocal(arenaInfo, new Vector2(0, -50), 1.5f).setEase(LeanTweenType.easeOutExpo).setDelay(1f);
            LeanTween.moveLocal(arenaInfo, new Vector2(0, 50), 0.5f).setEase(LeanTweenType.easeInExpo).setDelay(4f);
        }

        // Set boss destroyed event
        Events.active.onBossDestroyed += BossDestroyed;
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
        float time = EnemySpawner.GetTime();

        // Save the game
        SaveSystem.UpdateArena(arena.InternalID, time);
        SaveSystem.UpdateSave();
    }

    // Defeat boss
    public void BossDestroyed()
    {
        if (arena.useAchievementBoss && !awardGiven)
        {
            Debug.Log("Awarding boss achievement!");
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
