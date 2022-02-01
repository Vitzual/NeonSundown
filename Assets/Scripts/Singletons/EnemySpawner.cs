using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // List of all stages
    public List<Stage> stages;
    private Stage activeStage;
    private int nextStageIndex = 0;
    private bool stagesLeft = true;

    // List of all enemies with their cooldowns
    public Dictionary<EnemyData, float> enemies;

    // Game timer
    private float timer = 0;
    private float stageTime = 0;

    // Spawning flag
    public bool spawnEnemies = true;

    // On start, generate enemies
    void Start()
    {
        // Get enemies from scriptable controller
        enemies = new Dictionary<EnemyData, float>();
        foreach (EnemyData enemy in Scriptables.enemies)
            enemies.Add(enemy, 0);

        // Setup stages
        activeStage = stages[nextStageIndex];
        stageTime = activeStage.time;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

        // Increase run timer
        timer += Time.deltaTime;
        stageTime -= Time.deltaTime;

        // Check if next stage should start
        if (stagesLeft && stageTime <= 0) NextStage();

        // Check if enemy spawning enabled
        if (!spawnEnemies) return;

        // Spawn enemies
        foreach(Stage.Enemy enemy in activeStage.enemies)
        {
            if (enemies[enemy.data] <= 0f)
            {
                enemies[enemy.data] = enemy.cooldown;
                EnemyHandler.active.CreateEnemy(enemy.data);
            }
            else enemies[enemy.data] -= Time.deltaTime;
        }
    }

    private void NextStage()
    {
        // Check if stage exists
        if (stages.Count >= nextStageIndex)
        {
            Debug.Log("No stages left!");
            stagesLeft = false;
            return;
        }

        // Setup stages
        activeStage = stages[nextStageIndex];
        stageTime = activeStage.time;
        nextStageIndex += 1;
    }
}
