using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMissile : Bullet
{
    public WeaponData torpedo;
    private float cooldown = 0.25f;

    // Moves the bullet
    public override void Move()
    {
        // If cooldown expired, fire bullet
        if (cooldown < 0)
        {
            cooldown = 0.5f;
            BulletHandler.active.CreateBullet(parent, torpedo, transform.position, transform.rotation,
                weapon.bullets, weapon.bloom, weapon.material);
        }
        else cooldown -= Time.deltaTime;

        // Move the bullet
        base.Move();
    }

    // On collision
    public override void OnHit(Entity entity)
    {
        // Remove pierces
        pierce -= 1;
        entity.Damage(damage, knockback);

        // Check if entity dead
        if (!entity.IsDead() && stunLength > 0)
            entity.Stun(stunLength);

        // Check if bullet has a sound
        if (weapon.onDamageSound != null)
            AudioPlayer.Play(weapon.onDamageSound, true, weapon.minPitch, weapon.maxPitch);

        // Check if bullet has a sound
        if (weapon.onDeathSound != null && entity.IsDead())
            AudioPlayer.Play(weapon.onDeathSound, true, weapon.minPitch, weapon.maxPitch);

        // Check pierce amount
        if (pierce < 0)
        {
            // Get material to hold
            Material holder = entity.GetMaterial();

            // Check if entity overrides this particle
            if (entity.overrideOtherParticles)
            {
                deathMaterial = holder;
                deathEffect = entity.deathEffect;
            }

            deathMaterial = holder;
            Destroy();
        }
    }

    // Destroy the bullet
    public override void Destroy()
    {
        // Check if bullet is explosive
        if (stunLength > 0) ExplosiveHandler.CreateStun(transform.position, 10f, stunLength, damage, deathMaterial, knockback);
        else ExplosiveHandler.CreateExplosion(transform.position, 10f, damage, knockback, deathMaterial);

        // Destroy the bullet
        if (!explosive && weapon.useParticle) CreateParticle();
        Destroy(gameObject);
    }
}
