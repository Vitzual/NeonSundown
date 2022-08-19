using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // Custom enemy spawn queue
    public class EnemyQueue
    {
        public EnemyQueue(EnemyData enemy, Variant variant, 
            Vector2 position, bool disableRotation, bool enableLockOn)
        {
            this.variant = variant;
            this.enemy = enemy;
            this.position = position;
            this.disableRotation = disableRotation;
            this.enableLockOn = enableLockOn;
        }

        public Variant variant;
        public EnemyData enemy;
        public Vector2 position;
        public bool disableRotation, enableLockOn;
    }
    protected Queue<EnemyQueue> enemyQueue = new Queue<EnemyQueue>();

    // Active instance
    public static EnemyHandler active;

    // Player object
    public float spawnRange = 120;
    public float cullRange = 200;
    private int cullIndex = 0;
    
    // Contains all active enemies in the scene
    public List<Enemy> enemies = new List<Enemy>();
    public static float syphon = 0f;

    // Player transform for targetting
    public Ship ship;
    public Transform player;

    // On awake set active instance
    public void Awake() { active = this; syphon = 0f; }

    // Spawn enemies
    public void Update()
    {
        // Check if dealer is open
        if (Dealer.isOpen) return;

        // Check distance of enemy
        if (cullIndex < enemies.Count)
        {
            // Check if enemy exists
            if (enemies[cullIndex] == null)
                enemies.RemoveAt(cullIndex);

            // If enemy is far away, yeet and delete
            else if (enemies[cullIndex].isCullable && Vector2.Distance(enemies[cullIndex].transform.position, player.position) > cullRange)
            {
                Destroy(enemies[cullIndex].gameObject);
                enemies.RemoveAt(cullIndex);
            }

            // Update cull index
            cullIndex += 1;
        }
        else cullIndex = 0;

        // Enemy queue
        if (enemyQueue.Count > 0 && enemies.Count < 300)
        {
            // Dequeue enemy from list
            EnemyQueue newEnemy = enemyQueue.Dequeue();
            CreateEnemy(newEnemy.enemy, newEnemy.variant, newEnemy.position, 
                newEnemy.disableRotation, newEnemy.enableLockOn, true);
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
    
    /// <summary>
    /// Queues a new enemy for the arena to spawn
    /// </summary>
    /// <param name="enemyData"></param>
    /// <param name="variant"></param>
    /// <param name="amount"></param>
    /// <param name="disableRotation"></param>
    /// <param name="enableLockOn"></param>
    public void QueueEnemy(EnemyData enemyData, Variant variant, 
        int amount, bool disableRotation, bool enableLockOn)
    {
        // Check queue length
        if (enemyQueue.Count > 1000) return;

        // Generate position
        Vector2 position = GeneratePosition();

        // For loop
        for (int i = 0; i < amount; i++)
        {
            // Create slight offset
            if (i > 0) position = new Vector2(position.x + Random.Range(-5f, 5f), position.y + Random.Range(-5f, 5f));

            // Create the tile
            enemyQueue.Enqueue(new EnemyQueue(enemyData, variant, position, disableRotation, enableLockOn));
        }
    }

    /// <summary>
    /// Creates a new enemy at the specified location
    /// </summary>
    /// <param name="enemyData"></param>
    /// <param name="variant"></param>
    /// <param name="position"></param>
    /// <param name="disableRotation"></param>
    /// <param name="enableLockOn"></param>
    /// <param name="global"></param>
    public void CreateEnemy(EnemyData enemyData, Variant variant, Vector2 position, bool disableRotation, 
        bool enableLockOn, bool global, bool isClone = false, bool isSeeded = false)
    {
        // Create the tile
        Vector2 spawnPos;

        // Get spawn position
        if (global) spawnPos = new Vector2(player.position.x + position.x, player.position.y + position.y);
        else spawnPos = position;

        // Spawn the entity
        GameObject lastObj = Instantiate(enemyData.obj, spawnPos, Quaternion.identity);
        lastObj.name = enemyData.name;
        
        // Rotate to the target
        float angle = Mathf.Atan2(player.transform.position.y - lastObj.transform.position.y,
            player.transform.position.x - lastObj.transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        lastObj.transform.rotation = targetRotation;

        // Attempt to set enemy variant
        Enemy enemy = lastObj.GetComponent<Enemy>();
        enemy.Setup(enemyData, variant, player);
        if (isSeeded) enemy.SeedEnemy(ship);
        if (isClone) enemy.isClone = true;
        if (enableLockOn) enemy.EnableLockOn();
        else if (disableRotation) enemy.DisableRotation();

        // Add to enemies list
        enemies.Add(enemy);
    }

    // Generates a random location to spawn
    public Vector2 GeneratePosition()
    {
        // Create local position
        Vector2 localPos = new Vector2(0, 0);

        // Get location around border
        if (Random.value > 0.5f)
        {
            if (Random.value > 0.5f) return new Vector2(localPos.x + spawnRange, localPos.y + Random.Range(-spawnRange, spawnRange));
            else return new Vector2(localPos.x - spawnRange, localPos.y + Random.Range(-spawnRange, spawnRange));
        }
        else
        {
            if (Random.value > 0.5f) return new Vector2(localPos.x + Random.Range(-spawnRange, spawnRange), localPos.y + spawnRange);
            else return new Vector2(localPos.x + Random.Range(-spawnRange, spawnRange), localPos.y - spawnRange);
        }
    }

    // Returns the closest enemy
    public Enemy GetRandomEnemy()
    {
        if (enemies.Count == 0) return null;
        else return enemies[Random.Range(0, enemies.Count)];
    }
}