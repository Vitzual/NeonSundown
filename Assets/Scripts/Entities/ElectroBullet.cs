using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroBullet : Bullet
{
    public ParticleSystem electroParticle;

    public override void Setup(Weapon parent, WeaponData weapon, Material material, Transform target = null, 
        bool isSplitShot = false, bool explosiveRound = false, bool autoLock = false)
    {
        // Set electric bullet variables
        stunLength = 3f;
        ParticleSystemRenderer electroRenderer = electroParticle.GetComponent<ParticleSystemRenderer>();
        electroRenderer.material = weapon.material;
        electroRenderer.trailMaterial = weapon.material;

        base.Setup(parent, weapon, material, target, isSplitShot, explosiveRound, autoLock);
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
}
