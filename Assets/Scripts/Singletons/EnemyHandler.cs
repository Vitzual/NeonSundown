using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // Active instance
    public static EnemyHandler active;

    // Player object
    public float spawnRange = 100;

    // Contains all active enemies in the scene
    public List<Enemy> enemies = new List<Enemy>();

    // Player transform for targetting
    public Transform player;

    // On awake set active instance
    public void Awake() { active = this; }

    // Start method
    public void Start() { active = this; }

    // Move normal enemies
    public void FixedUpdate()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

        // Move enemies each frame towards their target
        for (int a = 0; a < enemies.Count; a++)
        {
            if (enemies[a] != null)
            {
                enemies[a].Move();
            }
            else
            {
                enemies.RemoveAt(a);
                a--;
            }
        }
    }

    // Creates a new enemy with a specific position
    public void CreateEnemy(EnemyData enemyData, Variant variant, Vector2 position)
    {
        // Create the tile
        GameObject lastObj = Instantiate(enemyData.obj, position, Quaternion.identity);
        lastObj.name = enemyData.name;

        // Attempt to set enemy variant
        Enemy enemy = lastObj.GetComponent<Enemy>();
        enemy.Setup(enemyData.variants[variant], variant, player);

        // Add to enemies list
        enemies.Add(enemy);
    }

    // Create a new active enemy instance
    public void CreateEnemy(EnemyData enemyData, Variant variant)
    {
        // Generate position
        Vector2 position = GeneratePosition();

        // Create the tile
        GameObject lastObj = Instantiate(enemyData.obj, position, Quaternion.identity);
        lastObj.name = enemyData.name;

        // Attempt to set enemy variant
        Enemy enemy = lastObj.GetComponent<Enemy>();
        enemy.Setup(enemyData.variants[variant], variant, player); 

        // Add to enemies list
        enemies.Add(enemy);
    }

    // Generates a random location to spawn
    public Vector2 GeneratePosition()
    {
        // Get location around border
        if (Random.value > 0.5f)
        {
            if (Random.value > 0.5f) return new Vector2(player.position.x + spawnRange, player.position.y + Random.Range(-spawnRange, spawnRange));
            else return new Vector2(player.position.x - spawnRange, player.position.y + Random.Range(-spawnRange, spawnRange));
        }
        else
        {
            if (Random.value > 0.5f) return new Vector2(player.position.x + Random.Range(-spawnRange, spawnRange), player.position.y + spawnRange);
            else return new Vector2(player.position.x + Random.Range(-spawnRange, spawnRange), player.position.y - spawnRange);
        }
    }

    // Returns the closest enemy
    public Enemy GetRandomEnemy()
    {
        if (enemies.Count == 0) return null;
        else return enemies[Random.Range(0, enemies.Count)];
    }
}