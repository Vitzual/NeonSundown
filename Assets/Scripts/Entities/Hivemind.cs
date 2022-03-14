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
    public override void Setup(VariantData data, Transform player)
    {
        cooldown = data.speed;
        base.Setup(data, player);
    }

    // Start is called before the first frame update
    public override void Move()
    {
        if (cooldown <= 0)
        {
            cooldown = data.speed;
            Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-randomizeRange, randomizeRange), 
                transform.position.y + Random.Range(-randomizeRange, randomizeRange));
            EnemyHandler.active.CreateEnemy(enemy, variant, spawnPos, false);
        }
        else cooldown -= Time.deltaTime;
    }
}
