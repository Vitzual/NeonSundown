using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoy : Bullet
{
    // Check if decoy has been triggered
    private List<Entity> entities = new List<Entity>();

    // On collision
    public override void OnHit(Entity entity)
    {
        // Check if entity is dead
        if (entity.IsDead()) return;

        // Check if entity has already hit decoy effect area
        if (entities.Contains(entity))
        {
            // Trigger like normal
            base.OnHit(entity);
        }
        else
        {
            // Set entity target
            if (entity is Enemy)
            {
                Enemy enemy = (Enemy)entity;
                enemy.SetTarget(transform);
                entities.Add(entity);
            }
        }
    }
}
