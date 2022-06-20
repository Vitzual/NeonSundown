using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Enemy
{
    // Spawning variables
    public EnemyData enemyToSpawn;
    public bool setupWithHandler;
    public bool circlePlayer = false;

    // Spawn cooldown;
    public float spawnCooldown;
    public Vector2 spawnOffset;
    private float spawnTimer;

    public override void Setup(EnemyData data, Variant variant, Transform target)
    {
        spawnTimer = spawnCooldown;
        base.Setup(data, variant, target);
    }

    public override void Move()
    {
        // Decrease timer
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            // Set the spawn timer
            spawnTimer = spawnCooldown;
            Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-spawnOffset.x, spawnOffset.x),
                                           transform.position.y + Random.Range(-spawnOffset.y, spawnOffset.y));

            // Check if enemy should be setup with handler
            if (setupWithHandler)
                EnemyHandler.active.CreateEnemy(enemyToSpawn, variant, spawnPos, false, false, false);

            // If not, just instantiate it
            else
            {
                Enemy newEnemy = Instantiate(enemyToSpawn.obj, spawnPos, transform.rotation).GetComponent<Enemy>();
                newEnemy.Setup(enemyToSpawn, variant, target);
            }
        }

        // Circle the player
        if (circlePlayer)
        {
            // Gradually rotate to the target
            float rotateStep = variantData.rotateSpeed * Time.deltaTime;
            float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
                target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateStep);

            // Move towards the target
            float movementStep = variantData.speed * Time.deltaTime;
            transform.position += transform.up * movementStep;
        }

        // If not circle, move normally
        base.Move();
    }
}
