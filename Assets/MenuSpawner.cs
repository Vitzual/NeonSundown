using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpawner : MonoBehaviour
{
    // Contains all active enemies in the scene
    public List<Enemy> enemies = new List<Enemy>();

    public float speed;
    public float rotate;
    public float range;
    public float moveUpTo;
    public float cooldown;
    private float timer;

    private void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if cooldown above 0
        if (timer > 0) timer -= Time.deltaTime;

        // If not, spawn enemy
        else
        {
            SpawnMenuEnemy();
            timer = cooldown;
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

    public void SpawnMenuEnemy()
    {
        // Iterate through enemies
        EnemyData newEnemy = Scriptables.enemies[Random.Range(0, Scriptables.enemies.Count)];
        Vector2 spawnPos = new Vector2(Random.Range(-range, range), transform.position.y);
        Enemy enemy = Instantiate(newEnemy.obj, spawnPos, Quaternion.identity).GetComponent<Enemy>();
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb != null) Destroy(rb);
        enemy.transform.name = newEnemy.name;
        enemy.Setup(newEnemy, null);
        enemies.Add(enemy);
    }
}
