using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaController : MonoBehaviour
{
    // Static instance
    public static ArenaController active;

    // List of all stages
    public StageData activeStage;
    private int nextStageIndex = 0;
    private bool stagesLeft = true;

    // Interface elements
    public TextMeshProUGUI timer;
    public TextMeshProUGUI stageText;

    // List of all enemies with their cooldowns
    public Dictionary<EnemyData, float> enemies;

    // Game timer
    private static float time = 0;
    private float stageTime = 0;
    private float scaleCalc = 0;
    private float scaleIncrease = 15;
    public static float enemyHealthMultiplier = 1;
    public static float enemySpeedMultiplier = 1;
    public static float enemyDamageMultiplier = 1;
    public static float crystalDropChance = 1;

    // Spawning flag
    public bool spawnEnemies = true;
    private bool awardGiven = false;

    // On awake set instance
    private void Awake()
    {
        // Set active instance
        active = this;
        awardGiven = false;

        // Reset the timer
        enemyHealthMultiplier = 1;
        enemySpeedMultiplier = 1;
        enemyDamageMultiplier = 1;
        crystalDropChance = 1;
        time = 0;
    }

    // On start, generate enemies
    void Start()
    {
        // Get enemies from scriptable controller
        enemies = new Dictionary<EnemyData, float>();
        foreach (EnemyData enemy in Scriptables.enemies)
            enemies.Add(enemy, 0);

        // Setup stages
        NextStage();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

        // Increase run timer
        time += Time.deltaTime;
        stageTime -= Time.deltaTime;

        // Update scaling
        if (activeStage.enemyScaling)
        {
            scaleIncrease -= Time.deltaTime;
            if (scaleIncrease <= 0)
            {
                scaleIncrease = 15f;
                scaleCalc += 0.25f;

                // At 10 minutes, start really ramping it up
                if (time > 600)
                {
                    enemyHealthMultiplier += 1f;
                    enemySpeedMultiplier += 0.05f;
                    enemyDamageMultiplier += 0.50f;
                }

                // Before 10 minutes, ramp up slowly
                else
                {
                    enemyHealthMultiplier += 0.3f;
                    enemySpeedMultiplier += 0.025f;
                    enemyDamageMultiplier += 0.15f;
                }

                // Lower crystal drop rate
                if (crystalDropChance > 0.25f)
                    crystalDropChance -= 0.02f;
            }
        }
        
        // Set the timer string
        timer.text = Formatter.Time(time);
        if (Gamemode.arena.useAchievementTime && time > Gamemode.arena.achievementTime && !awardGiven)
        {
            Debug.Log("Awarding achievement!");
            awardGiven = true;
            if (!Gamemode.arena.achievement.IsAchieved)
            {
                Gamemode.arena.achievement.Unlock();
                Gamemode.arena.achievement.Store();
            }
            else Debug.Log("Achievement has already been given!");
        }

        // Check if next stage should start
        if (stagesLeft && stageTime <= 0) NextStage();

        // Check if enemy spawning enabled
        if (!spawnEnemies) return;

        // Spawn enemies
        foreach(StageData.Enemy enemy in activeStage.enemies)
        {
            if (enemies[enemy.data] <= 0f)
            {
                enemies[enemy.data] = enemy.cooldown;
                if (activeStage.enemyScaling)
                {
                    enemies[enemy.data] -= scaleCalc;
                    if (enemies[enemy.data] < 0.1f)
                        enemies[enemy.data] = 0.1f;
                }
                EnemyHandler.active.QueueEnemy(enemy.data, enemy.variant, 
                    enemy.amount, enemy.disableRotation, enemy.enableLockOn);
            }
            else enemies[enemy.data] -= Time.deltaTime;
        }
    }

    private void NextStage()
    {
        // Check if stage exists
        if (Gamemode.arena.stages.Count <= nextStageIndex)
        {
            Debug.Log("No stages left!");
            stagesLeft = false;
            return;
        }

        // Setup stages
        activeStage = Gamemode.arena.stages[nextStageIndex];
        stageText.text = activeStage.name;
        stageTime = activeStage.time;
        nextStageIndex += 1;
        scaleCalc = 0;
        scaleIncrease = 15;
    }

    // Returns the time
    public static float GetTime() { return time; }
}
