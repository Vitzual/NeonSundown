using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{
    // Spawning variables
    public EnemyData enemyToSpawn;
    public bool setupWithHandler;

    // Spawn cooldown;
    public float spawnCooldown;
    private float spawnTimer;

    public override void Setup(VariantData data, Transform player)
    {
        spawnTimer = spawnCooldown;
        base.Setup(data, player);
    }

    public override void Move()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            // Set the spawn timer
            spawnTimer = spawnCooldown;

            // Check if enemy should be setup with handler
            if (setupWithHandler)
                EnemyHandler.active.CreateEnemy(enemyToSpawn, variant, transform.position);

            // If not, just instantiate it
            else
            {
                Enemy newEnemy = Instantiate(enemyToSpawn.obj, transform.position, transform.rotation).GetComponent<Enemy>();
                newEnemy.Setup(enemyToSpawn.variants[data.variant], null);
            }
        }

        base.Move();
    }
}
