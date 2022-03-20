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
    public override void Damage(float amount, float knockback = 0f)
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
            if (shieldActive)
            {
                Vector3 currentRotation = bullet.transform.eulerAngles;
                bullet.transform.eulerAngles = new Vector3(-currentRotation.x, -currentRotation.y, currentRotation.z);
            }
            else
            {
                bullet.deathMaterial = material;
                Damage(bullet.GetDamage());
                bullet.Destroy();
                return;
            }
        }
    }

    // Disable shield
    public override void Stun(float length)
    {
        // Damage shield if active
        if (shieldActive)
        {
            AudioPlayer.Play(shieldSound, true, 0.8f, 0.8f);
            shieldActive = false;
            shield.SetActive(false);
        }
    }
}
