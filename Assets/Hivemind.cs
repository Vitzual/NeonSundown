using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hivemind : Enemy
{
    // Cooldown
    public EnemyData enemy;
    public float randomizeRange = 10;
    private float cooldown;

    // Setup the hivemind
    public override void Setup(EnemyData data, Transform player)
    {
        cooldown = data.speed;
        base.Setup(data, player);
    }

    // Start is called before the first frame update
    public override void Move()
    {
        if (cooldown <= 0)
        {
            cooldown = enemyData.speed;
            Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-randomizeRange, randomizeRange), 
                transform.position.y + Random.Range(-randomizeRange, randomizeRange));
            EnemyHandler.active.CreateEnemy(enemy, spawnPos);
        }
        else cooldown -= Time.deltaTime;
    }
}
