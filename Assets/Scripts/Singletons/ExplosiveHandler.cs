using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveHandler : MonoBehaviour
{
    // Explosion variables
    public AudioClip _explosionSound;
    public AudioClip _stunSound;
    public ParticleSystem _explosionParticle;
    public ParticleSystem _inverseParticle;
    public ParticleSystem _stunParticle;
    public List<LayerMask> _explosionLayers;
    public static AudioClip explosionSound;
    public static AudioClip inverseSound;
    public static AudioClip stunSound;
    public static ParticleSystem explosionParticle;
    public static ParticleSystem inverseParticle;
    public static ParticleSystem stunParticle;
    public static LayerMask explosionLayer;
    public static bool inverse = false;

    // On start get variables
    public void Start()
    {
        explosionSound = _explosionSound;
        stunSound = _stunSound;
        explosionParticle = _explosionParticle;
        inverseParticle = _inverseParticle;
        stunParticle = _stunParticle;
        inverse = false;

        foreach (LayerMask layer in _explosionLayers)
            explosionLayer = layer | explosionLayer;
    }

    // Return a list of entities
    public static Collider2D[] CastForEntities(Vector2 position, float range)
    {
        // Get all colliders in range
        return Physics2D.OverlapCircleAll(position, range, explosionLayer);
    }

    // Create an explosion at a location
    public static void CreateExplosion(Vector2 position, float range, float damage, float knockback, Material material)
    {
        // Get all colliders in range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, range, explosionLayer);

        // Check if knockback should be inverted
        if (inverse) knockback = -knockback;

        // Iterate through colliders and do the damage
        for (int i = 0; i < colliders.Length; i++)
        {
            // Get enemy component
            Enemy enemy = colliders[i].GetComponent<Enemy>();

            // If enemy not null, apply damage
            if (enemy != null)
                enemy.Damage(damage, knockback, position);

            // If null, check for crystals
            else
            {
                // Get crystal component
                Crystal crystal = colliders[i].GetComponent<Crystal>();

                // If crystal not null, apply damage
                if (crystal != null)
                    crystal.Damage(damage, knockback, position);
            }
        }

        // Create particle effect
        if (Settings.useParticles)
        {
            ParticleSystemRenderer holder;
            if (inverse)
            {
                holder = Instantiate(inverseParticle, position,
                    Quaternion.identity).GetComponent<ParticleSystemRenderer>();
            }
            else
            {
                holder = Instantiate(explosionParticle, position, 
                    Quaternion.identity).GetComponent<ParticleSystemRenderer>();
            }
            holder.material = material;
            holder.trailMaterial = material;
        }

        // Play explosion sound
        if (inverse) AudioPlayer.Play(explosionSound);
        else AudioPlayer.Play(explosionSound);
    }

    // Create a knockback without explosion
    public static void CreateKnockback(Vector2 origin, float range, float minKnockback, float maxKnockback, float damage = 0f)
    {
        // Get all colliders in range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, range, explosionLayer);

        // Iterate through colliders and do the damage
        for (int i = 0; i < colliders.Length; i++)
        {
            // Get enemy component
            Enemy enemy = colliders[i].GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Knockback(Random.Range(minKnockback, maxKnockback), origin);
                if (damage > 0) enemy.Damage(damage);
            }

            // Get crystal component
            else 
            {
                // Get crystal component
                Crystal crystal = colliders[i].GetComponent<Crystal>();
                if (crystal != null)
                {
                    crystal.Knockback(Random.Range(minKnockback, maxKnockback), origin);
                    if (damage > 0) crystal.Damage(damage);
                }
            }
        }
    }

    public static void CreateStun(Vector2 origin, float range, float length, float damage, Material material, float knockback = -1f)
    {
        // Get all colliders in range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, range, explosionLayer);

        // Iterate through colliders and do the damage
        for (int i = 0; i < colliders.Length; i++)
        {
            // Get enemy component
            Enemy enemy = colliders[i].GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage(damage, knockback, origin);
                enemy.Stun(length);
            }

            // If null, check for crystals
            else
            {
                // Get crystal component
                Crystal crystal = colliders[i].GetComponent<Crystal>();

                // If crystal not null, apply damage
                if (crystal != null)
                    crystal.Damage(damage, knockback, origin);
            }
        }

        // Create particle effect
        if (Settings.useParticles)
        {
            ParticleSystemRenderer holder = Instantiate(stunParticle, origin,
                    Quaternion.identity).GetComponent<ParticleSystemRenderer>();
            holder.material = material;
            holder.trailMaterial = material;
        }

        // Play stun sound
        AudioPlayer.Play(stunSound, true, 0.9f, 1.1f, false, 0.5f);
    }
}
