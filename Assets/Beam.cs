using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Laser
{
    // Particle system
    public new ParticleSystem particleSystem;
    public ParticleSystemRenderer particleRenderer;
    public AudioSource audioSource;

    // Update beam timer
    private float timer, endAtTime, endEarlyTime, damageCooldown = 0.5f, damageTimer = 0f;

    public void SetupBeam(Transform parent, float width, float length, float spread,
        float pierces, AudioClip sound, Material beamMaterial, bool useSound)
    {
        // Set damage cooldown
        if (pierces > 0) damageCooldown = 1f / pierces;
        else damageCooldown = 1f;

        // Setup sound effect
        audioSource.clip = sound;
        if (useSound) audioSource.volume = Settings.sound;
        else audioSource.volume = 0f;
        audioSource.Play();

        // Set beam variables
        timer = 0f;
        endAtTime = sound.length;
        endEarlyTime = sound.length - 1.5f;

        // Setup beam effects
        particleRenderer.trailMaterial = beamMaterial;
        particleRenderer.material = beamMaterial;
        SetupLaser(parent, width, length, spread);
    }

    // On hit, apply damage
    public override void OnHit(Entity entity)
    {
        // Remove pierces
        entity.Damage(damage, knockback);

        // Check if entity dead
        if (!entity.IsDead() && stunLength > 0)
            entity.Stun(stunLength);

        // Check if bullet is explosive
        if (explosive)
        {
            if (stunLength > 0) ExplosiveHandler.CreateStun(entity.transform.position, explosionSize, stunLength, damage, deathMaterial, knockback);
            else ExplosiveHandler.CreateExplosion(entity.transform.position, explosionSize, damage, knockback, deathMaterial);
        }

        // Check if bullet is splitshot
        if (!isSplitShot) BulletHandler.active.CreateSplitshot(parent, weapon, transform.position,
            transform.rotation, bulletSize / 2, (int)splitshots, normalMaterial, 360f, explosive);
    }

    public override void Move()
    {
        // Iterate through entities
        if (damageTimer <= 0f)
        {
            damageTimer = damageCooldown;
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] != null)
                {
                    OnHit(entities[i]);
                }
                else
                {
                    entities.RemoveAt(i);
                    i--;
                }
            }
        }
        else damageTimer -= Time.deltaTime;

        timer += Time.deltaTime;
        if (timer >= endAtTime) Destroy();
        else if (timer >= endEarlyTime) base.Move();
    }

    public void EndEarly()
    {
        if (timer < endEarlyTime)
        {
            timer = endEarlyTime;
            audioSource.time = timer;
            particleSystem.Stop();
            Events.active.ResetCooldown();
        }
    }

    public override void AddEntity(Entity entity)
    {
        OnHit(entity);
        base.AddEntity(entity);
    }
}
