using System.Collections;
using System.Collections.Generic;
using Thinksquirrel.CShake;
using UnityEngine;

public class Enemy : Entity
{
    // Scriptable object
    private EnemyData enemyData;

    // Internal runtime variables
    private float health;
    private float maxHealth;

    // Target transform for moving
    private Transform target;
    
    // Setup the enemy
    public virtual void Setup(EnemyData data, Transform player)
    {
        // Set scriptable
        enemyData = data;

        // Set material / particle
        deathEffect = data.deathParticle;
        deathMaterial = data.material;

        // Set stats
        health = enemyData.health;
        maxHealth = health;

        // Set target
        target = player;
    }

    // Damage entity
    public void Damage(float amount)
    {
        health -= amount;
        if (health <= 0) Destroy();
    }

    // Destroy entity
    public virtual void Destroy()
    {
        CreateParticle();
        XPHandler.active.Spawn(transform.position, enemyData.xpDrops);
        CameraShake.ShakeAll();
        Destroy(gameObject);
    }

    // Move towards the target
    public virtual void Move()
    {
        if (target != null)
        {
            // Rotate to the target
            float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, 
                target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            transform.rotation = targetRotation;

            // Move towards the target
            float step = enemyData.speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }

    // Get material function
    public Material GetMaterial()
    {
        return enemyData.material;
    }
}
