using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Static instance
    public static EnemySpawner active;

    // List of all stages
    public List<StageData> stages;
    private StageData activeStage;
    private int nextStageIndex = 0;
    private bool stagesLeft = true;

    // Interface elements
    public TextMeshProUGUI timer;
    public TextMeshProUGUI stageText;

    // List of all enemies with their cooldowns
    public Dictionary<EnemyData, float> enemies;

    // Game timer
    private float time = 0;
    private float stageTime = 0;

    // Spawning flag
    public bool spawnEnemies = true;

    // On awake set instance
    private void Awake()
    {
        // Set active instance
        active = this;
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

        // Set the timer string
        timer.text = Formatter.Time(time);

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
                EnemyHandler.active.CreateEnemy(enemy.data, enemy.variant);
            }
            else enemies[enemy.data] -= Time.deltaTime;
        }
    }

    private void NextStage()
    {
        // Check if stage exists
        if (stages.Count <= nextStageIndex)
        {
            Debug.Log("No stages left!");
            stagesLeft = false;
            return;
        }

        // Setup stages
        activeStage = stages[nextStageIndex];
        stageText.text = activeStage.name;
        stageTime = activeStage.time;
        nextStageIndex += 1;
    }
}
