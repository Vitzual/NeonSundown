using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpawner : MonoBehaviour
{
    // Menu stage
    public StageData menuStage;

    // List of all enemies with their cooldowns
    public Dictionary<EnemyData, float> enemyList;
    public List<Enemy> enemies;

    public float range;
    public float moveUpTo;

    // On start, generate enemies
    void Start()
    {
        // Get enemies from scriptable controller
        enemyList = new Dictionary<EnemyData, float>();
        foreach (EnemyData enemy in Scriptables.enemies)
            enemyList.Add(enemy, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn enemies
        foreach (StageData.Enemy enemy in menuStage.enemies)
        {
            if (enemyList[enemy.data] <= 0f)
            {
                enemyList[enemy.data] = enemy.cooldown;
                SpawnMenuEnemy(enemy.data);
            }
            else enemyList[enemy.data] -= Time.deltaTime;
        }

        // Move enemies each frame towards their target
        for (int a = 0; a < enemies.Count; a++)
        {
            if (enemies[a] != null)
            {
                // Rotate if it says to 
                EnemyData enemyData = enemies[a].GetData();
                if (enemyData.rotate) enemies[a].rotator.Rotate(Vector3.forward, enemyData.rotateSpeed * Time.deltaTime);

                // Move towards the target
                float step = enemyData.speed * Time.deltaTime;
                enemies[a].transform.position = Vector2.MoveTowards(enemies[a].transform.position, 
                    new Vector2(enemies[a].transform.position.x, moveUpTo), step);

                // Check if above threshold
                if (enemies[a].transform.position.y >= moveUpTo)
                {
                    Destroy(enemies[a].gameObject);
                    enemies.RemoveAt(a);
                    a--;
                }
            }
            else
            {
                enemies.RemoveAt(a);
                a--;
            }
        }
    }

    public void SpawnMenuEnemy(EnemyData newEnemy)
    {
        // Setup the enemy
        Vector2 spawnPos = new Vector2(Random.Range(-range, range), transform.position.y);
        Enemy enemy = Instantiate(newEnemy.obj, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null) Destroy(rb);
        enemy.transform.name = newEnemy.name;
        enemy.Setup(newEnemy, null);
        enemies.Add(enemy);
    }
}
