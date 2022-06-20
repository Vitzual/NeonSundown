using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : Enemy
{
    // Shield stats
    public Transform shield;
    public ParticleSystem _particle;
    private ParticleSystem.MainModule particle;
    public AudioClip shieldSound;
    public AudioClip reflectSound;
    private bool shieldActive = true;
    private float initialSize = 7f;

    // Get initial shield size
    public override void Setup(EnemyData data, Variant variant, Transform target)
    {
        initialSize = shield.localScale.x;
        base.Setup(data, variant, target);
    }

    // Damage entity
    public override void Damage(float amount, float knockback = 0f)
    {
        // Damage shield if active
        if (!shieldActive) base.Damage(amount);
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
                bullet.ReverseBullet();
                float adjust = bullet.GetDamage() / 10;
                shield.localScale = new Vector3(shield.localScale.x 
                    - adjust, shield.localScale.y - adjust, 1);
                particle = _particle.main;
                particle.startLifetime = (shield.localScale.x / initialSize) - 0.25f;
                if (shield.localScale.x < 2f) DisableShield();
            }
            else
            {
                bullet.deathMaterial = material;
                Damage(bullet.GetDamage());
                bullet.Destroy();
            }
        }
    }

    // Disable shield
    public override void Stun(float length)
    {
        // Damage shield if active
        if (shieldActive) DisableShield();
    }

    // Turn off shield
    public void DisableShield()
    {
        shield.localScale = new Vector3(1f, 1f, 1f);
        AudioPlayer.Play(shieldSound, true, 0.8f, 0.8f);
        shield.gameObject.SetActive(false);
        shieldActive = false;
    }
}
