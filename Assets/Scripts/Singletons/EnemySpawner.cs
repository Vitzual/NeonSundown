using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Spawning flag
    public bool spawnEnemies = true;

    // Tracking
    private float timer;
    private float cooldown;
    public float _cooldown;

    private void Start()
    {
        timer = 0f;
        cooldown = _cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        // Increase run timer
        timer += Time.deltaTime;

        // Check if enemy spawning enabled
        if (!spawnEnemies) return;

        // Check if cooldown above 0
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            return;
        }
        else cooldown = _cooldown;

        // Iterate through enemies
        foreach(EnemyData enemy in Scriptables.enemies)
        {
            if (enemy.spawnTime < timer)
            {
                int random = Random.Range(0, 100);
                if (enemy.spawnChance > random)
                    EnemyHandler.active.CreateEnemy(enemy);
            }
        }
    }
}
