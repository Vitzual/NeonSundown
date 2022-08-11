using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnEnemyCollision : MonoBehaviour
{
    private Enemy parent;
    private float health = 0f;

    public void Setup(Enemy enemy)
    {
        health = enemy.GetHealth();
        parent = enemy;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null)
        {
            float enemyHealth = enemy.GetHealth();
            if (enemyHealth > health)
            {
                enemy.Damage(health, -500f, true);
                parent.Destroy();
            }
            else
            {
                enemy.Destroy();
                health -= enemyHealth;
            }
        }
    }
}
