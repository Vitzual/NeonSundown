using System.Collections;
using System.Collections.Generic;
using Thinksquirrel.CShake;
using UnityEngine;

public class Enemy : Entity
{
    // Scriptable object
    private EnemyData enemyData;

    // Rotation thing
    public Transform rotator;

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
    public override void Damage(float amount)
    {
        health -= amount;
        if (IsDead()) Destroy();
    }

    // Destroy entity
    public override void Destroy()
    {
        CreateParticle();
        XPHandler.active.Spawn(transform.position, enemyData.minXP);
        CameraShake.ShakeAll();
        Destroy(gameObject);
    }

    // Move towards the target
    public virtual void Move()
    {
        if (target != null)
        {
            if (enemyData.rotate)
            {
                // Rotate if it says to 
                rotator.Rotate(Vector3.forward, enemyData.rotateSpeed * Time.deltaTime);
            }
            else
            {
                // Rotate to the target
                float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
                    target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
                transform.rotation = targetRotation;
            }

            // Move towards the target
            float step = enemyData.speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }

    // Get material function
    public override Material GetMaterial()
    {
        return enemyData.material;
    }

    // Check if enemy is dead
    public bool IsDead()
    {
        return health <= 0;
    }

    // Get enemy data
    public EnemyData GetData()
    {
        return enemyData;
    }

    // On collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the other enemy component
        Bullet bullet = collision.GetComponent<Bullet>();

        // If is enemy, damage
        if (bullet != null)
            bullet.OnHit(this);
    }
}
