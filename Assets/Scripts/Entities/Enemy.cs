using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Scriptable object
    public EnemyData enemyData;

    // Internal runtime variables
    private float health;
    private float maxHealth;

    // Target transform for moving
    public Transform target;
    
    // Setup the enemy
    public virtual void Setup(EnemyData data)
    {
        enemyData = data;
        health = enemyData.health;
        maxHealth = health;
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
        Destroy(gameObject);
    }

    // Move towards the target
    public virtual void MoveTowardsTarget()
    {
        float step = enemyData.speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
    }
}
