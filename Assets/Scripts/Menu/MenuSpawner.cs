using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpawner : MonoBehaviour
{
    // Custom enemy spawn queue
    public class EnemyQueue
    {
        public EnemyQueue(EnemyData enemy, Variant variant, Vector2 position)
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
    public static MenuSpawner active;

    // Menu stage
    public StageData menuStage;

    // List of all enemies with their cooldowns
    public Dictionary<EnemyData, float> enemyList;
    public List<Enemy> enemies;

    public float range;
    public float moveUpTo;

    // On activate, get instance
    private void Awake()
    {
        active = this;
    }

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
        // Check if stage data null
        if (menuStage == null) return;

        // Spawn enemies
        if (enemyQueue.Count > 0)
        {
            EnemyQueue newEnemy = enemyQueue.Dequeue();
            CreateEnemy(newEnemy.enemy, newEnemy.variant, newEnemy.position);
        }

        // Spawn enemies
        foreach (StageData.Enemy enemy in menuStage.enemies)
        {
            if (enemyList[enemy.data] <= 0f)
            {
                enemyList[enemy.data] = enemy.cooldown;
                QueueEnemy(enemy.data, enemy.variant, enemy.amount);
            }
            else enemyList[enemy.data] -= Time.deltaTime;
        }

        // Move enemies each frame towards their target
        for (int a = 0; a < enemies.Count; a++)
        {
            if (enemies[a] != null)
            {
                // Rotate if it says to 
                VariantData data = enemies[a].GetVariantData();
                if (data.rotate) enemies[a].rotator.Rotate(Vector3.forward, data.rotateSpeed * Time.deltaTime);

                // Move towards the target
                float step = data.speed * Time.deltaTime;
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

    // Create a new active enemy instance
    public void QueueEnemy(EnemyData enemyData, Variant variant, int amount)
    {
        // Generate position
        Vector2 position = new Vector2(Random.Range(-range, range), transform.position.y);

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
        // Setup the enemy
        Enemy enemy = Instantiate(enemyData.obj, position, Quaternion.identity).GetComponent<Enemy>();
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null) rb.freezeRotation = true;
        enemy.Setup(enemyData, variant, null);
        enemies.Add(enemy);
    }
    
    // Wipe all enemies
    public void WipeEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
            enemies.RemoveAt(i);
            i--;
        }
    }
}
