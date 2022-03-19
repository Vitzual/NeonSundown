using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroBullet : Bullet
{
    public ParticleSystem electroParticle;
    public float castRange;

    public override void Setup(Weapon parent, WeaponData weapon, Material material, Transform target = null, 
        bool isSplitShot = false, bool explosiveRound = false)
    {
        stunLength = 3f;
        ParticleSystemRenderer electroRenderer = electroParticle.GetComponent<ParticleSystemRenderer>();
        electroRenderer.material = weapon.material;
        electroRenderer.trailMaterial = weapon.material;
        base.Setup(parent, weapon, material, target, isSplitShot, explosiveRound);
    }

    public override void Move()
    {
        // Decay bullet
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) Destroy();
        
        // Lock the target
        if (target != null)
            RotateToTarget(true);

        // Move forward
        transform.position += transform.up * speed * Time.deltaTime;

        // Check if spinning
        if (weapon.rotate)
            rotator.Rotate(Vector3.forward, weapon.rotateSpeed * Time.deltaTime);
    }

    public override void OnHit(Entity entity)
    {
        // Set target to null and cast for entities
        Collider2D[] colliders = ExplosiveHandler.CastForEntities(transform.position, castRange);

        // Loop through colliders to find new target
        bool newTargetFound = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            Transform newTarget = colliders[i].GetComponent<Transform>();
            if (newTarget != target)
            {
                newTargetFound = true;
                target = newTarget;
                break;
            }
        }
        if (!newTargetFound) target = null;
        
        // Call normal on hit
        base.OnHit(entity);
    }
}
