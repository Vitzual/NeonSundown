using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : Enemy
{
    // Shield stats
    public GameObject shield;
    public float shieldHealth;
    public AudioClip shieldSound;
    private bool shieldActive = true;

    // Damage entity
    public override void Damage(float amount)
    {
        // Damage shield if active
        if (shieldActive)
        {
            shieldHealth -= amount;
            if (shieldHealth <= 0)
            {
                AudioPlayer.Play(shieldSound, true, 0.8f, 0.8f);
                shieldActive = false;
                shield.SetActive(false);
            }
        }
        else base.Damage(amount);
    }

    // On collision
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if shield active
        if (!shieldActive) base.OnTriggerEnter2D(collision);

        // Get the other enemy component
        Bullet bullet = collision.GetComponent<Bullet>();

        // If is bullet, invoke on hit method
        if (bullet != null)
        {
            bullet.deathMaterial = material;
            Damage(bullet.GetDamage());
            bullet.Destroy();
            return;
        }
    }
}
