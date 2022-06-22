using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torrent : Enemy
{
    // List of enemies 
    private List<Enemy> enemies;
    public float dupeCooldown = 1;
    private float cooldown = 1, spawns = 0;

    // Setup new enemies list
    public override void Setup(EnemyData data, Variant variant, Transform target)
    {
        enemies = new List<Enemy>();
        cooldown = dupeCooldown;
        spawns = 0;
        base.Setup(data, variant, target);
    }

    // Custom update method
    public override void Move()
    {
        // Check if enemies should be duped
        cooldown -= Time.deltaTime;
        spawns = 0;
        if (cooldown <= 0)
        {
            cooldown = dupeCooldown;
            for (int i = 0; i < enemies.Count; i++)
            {
                // Break loop after 10 spawns
                if (spawns >= 10) break;

                // Spawn enemies
                if (enemies[i] != null)
                {
                    EnemyHandler.active.CreateEnemy(enemies[i].GetEnemyData(), enemies[i].GetVariant(),
                        transform.position, enemies[i].RotationEnabled(), enemies[i].LockOnEnabled(), false, true);
                    spawns += 1;
                }
                else
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
        }

        base.Move();
    }

    // On collision
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // Get enemy component 
        Enemy enemy = collision.GetComponent<Enemy>();

        // Check if enemy is null
        if (enemy != null && !enemy.isBoss && !enemy.isClone)
        {
            enemies.Add(enemy);
            enemy.isClone = true;
        }
        else
        {
            // Get the other enemy component
            Bullet bullet = collision.GetComponent<Bullet>();

            // If is bullet, invoke on hit method
            if (bullet != null)
            {
                bullet.deathMaterial = material;
                Damage(bullet.GetDamage());
                bullet.Destroy();
            }
        }
    }

    // On leave
    public void OnTriggerExit2D(Collider2D collision)
    {
        // Get enemy component 
        Enemy enemy = collision.GetComponent<Enemy>();

        // Check if enemy is null
        if (enemy != null && enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
            enemy.isClone = false;
        }
    }
}
