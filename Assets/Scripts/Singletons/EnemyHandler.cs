using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // Custom enemy spawn queue
    public class EnemyQueue
    {
        public EnemyQueue( EnemyData enemy, Variant variant, Vector2 position)
        {
            this.variant = variant;
            this.enemy = enemy;
            this.position = position;
        }

        public Variant variant;
        public EnemyData enemy;
        public Vector2 position;
    }
    protected Queue<EnemyQueue> enemyQueue = new Queue<EnemyQueue>();

    // Active instance
    public static EnemyHandler active;

    // Player object
    public float spawnRange = 100;
    public float cullRange = 150;
    private int cullIndex = 0;

    // Contains all active enemies in the scene
    public List<Enemy> enemies = new List<Enemy>();

    // Player transform for targetting
    public Transform player;

    // On awake set active instance
    public void Awake() { active = this; }

    // Start method
    public void Start() { active = this; }

    // Spawn enemies
    public void Update()
    {
        // Check distance of enemy
        if (cullIndex < enemies.Count)
        {
            // Check if enemy exists
            if (enemies[cullIndex] == null)
                enemies.RemoveAt(cullIndex);

            // If enemy is far away, yeet and delete
            else if (Vector2.Distance(enemies[cullIndex].transform.position, player.position) > cullRange)
            {
                Destroy(enemies[cullIndex].gameObject);
                enemies.RemoveAt(cullIndex);
            }

            // Update cull index
            cullIndex += 1;
        }
        else cullIndex = 0;

        // Enemy queue
        if (enemyQueue.Count > 0)
        {
            // Dequeue enemy from list
            EnemyQueue newEnemy = enemyQueue.Dequeue();
            CreateEnemy(newEnemy.enemy, newEnemy.variant, newEnemy.position);
        }
    }

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

    // Create a new active enemy instance
    public void QueueEnemy(EnemyData enemyData, Variant variant, int amount)
    {
        // Generate position
        Vector2 position = GeneratePosition();

        // For loop
        for (int i = 0; i < amount; i++)
        {
            // Create slight offset
            if (i > 0) position = new Vector2(position.x + Random.Range(-5f, 5f), position.y + Random.Range(-5f, 5f));

            // Create the tile
            enemyQueue.Enqueue(new EnemyQueue(enemyData, variant, position));
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
        enemy.Setup(enemyData.variants[variant], player);

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