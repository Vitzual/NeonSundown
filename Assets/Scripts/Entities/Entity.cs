using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // The particle effect for this bullet
    public bool overrideOtherParticles = false;
    public bool rotateParticle = false;
    public Material deathMaterial;
    public ParticleSystem deathEffect;

    // Creates a particle and sets the material
    public void CreateParticle()
    {
        // Check if has death effect
        if (deathEffect == null) return;

        // Create the effect
        ParticleSystemRenderer holder = Instantiate(deathEffect, transform.position,
                transform.rotation).GetComponent<ParticleSystemRenderer>();
        holder.material = deathMaterial;
        holder.trailMaterial = deathMaterial;

        // Rotate if set to true
        if (rotateParticle) holder.transform.rotation *= Quaternion.Euler(-90, 90, 0);
    }

    // Damage entity
    public virtual void Damage(float amount, float knockback = -10f)
    {
        // Do something
    }

    // Destroy entity
    public virtual void Destroy()
    {
        // Destroy object
        Destroy(gameObject);
    }

    // Check if enemy is dead
    public virtual bool IsDead()
    {
        return false;
    }

    // Get material function
    public virtual Material GetMaterial()
    {
        // Return material
        return deathMaterial;
    }
}
