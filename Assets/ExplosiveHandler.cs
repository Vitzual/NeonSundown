using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveHandler : MonoBehaviour
{
    // Explosion variables
    public AudioClip _explosionSound;
    public ParticleSystem _explosionParticle;
    public List<LayerMask> _explosionLayers;
    public static AudioClip explosionSound;
    public static ParticleSystem explosionParticle;
    public static LayerMask explosionLayer;

    // On start get variables
    public void Start()
    {
        explosionSound = _explosionSound;
        explosionParticle = _explosionParticle;

        foreach(LayerMask layer in _explosionLayers)
            explosionLayer = layer | explosionLayer;
    }

    // Create an explosion at a location
    public static void CreateExplosion(Vector2 position, float range, float damage, float knockback, Material material)
    {
        // Get all colliders in range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, range, explosionLayer);

        // Iterate through colliders and do the damage
        for(int i = 0; i < colliders.Length; i++)
        {
            // Get enemy component
            Enemy enemy = colliders[i].GetComponent<Enemy>();

            // If enemy not null, apply damage
            if (enemy != null)
                enemy.Damage(damage, knockback - 100, position);
        }

        // Create particle effect
        ParticleSystemRenderer holder = Instantiate(explosionParticle, 
            position, Quaternion.identity).GetComponent<ParticleSystemRenderer>();
        holder.material = material;
        holder.trailMaterial = material;

        // Play explosion sound
        AudioPlayer.Play(explosionSound);
    }
}
