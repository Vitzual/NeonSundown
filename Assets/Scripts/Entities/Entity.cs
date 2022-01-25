using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // The particle effect for this bullet
    protected Material deathMaterial;
    protected ParticleSystem deathEffect;

    // Creates a particle and sets the material
    public void CreateParticle(bool rotate = false)
    {
        // Check if has death effect
        if (deathEffect == null) return;

        // Create the effect
        ParticleSystemRenderer holder = Instantiate(deathEffect, transform.position,
                transform.rotation).GetComponent<ParticleSystemRenderer>();
        holder.material = deathMaterial;
        holder.trailMaterial = deathMaterial;

        // Rotate if set to true
        if (rotate) holder.transform.rotation *= Quaternion.Euler(-90, 90, 0);
    }
}
