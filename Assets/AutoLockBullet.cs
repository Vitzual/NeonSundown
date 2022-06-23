using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoLockBullet : Bullet
{
    // Range component
    public CircleCollider2D autoLockRange;

    // Auto lock flag
    private bool autoLocked = false;

    // On collision
    public override void OnHit(Entity entity)
    {
        // Check if locked
        if (!autoLocked)
        {
            autoLockRange.enabled = false;
            target = entity.transform;
            tracking = true;
            autoLocked = true;
        }
        else OnHit(entity);
    }
}
