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
    public override void Setup(EnemyData data, Variant variant, Transform target)
    {
        cooldown = data.variants[variant].speed;
        base.Setup(data, variant, target);
    }

    // Start is called before the first frame update
    public override void Move()
    {
        if (cooldown <= 0)
        {
            cooldown = variantData.speed;
            Vector2 spawnPos = new Vector2(transform.position.x + Random.Range(-randomizeRange, randomizeRange), 
                transform.position.y + Random.Range(-randomizeRange, randomizeRange));
            EnemyHandler.active.CreateEnemy(enemy, variant, spawnPos, false, true, false);
        }
        else cooldown -= Time.deltaTime;
    }
}
