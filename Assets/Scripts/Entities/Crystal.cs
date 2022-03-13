using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Entity
{
    // Crystal data
    public List<CrystalData> crystals;
    public SpriteRenderer crystalBorder;
    private CrystalData crystalData;

    // Internal values
    public Rigidbody2D rb;
    public AudioClip crystalSound;
    public AudioClip crystalDestroy;
    private float health = 50;
    private bool isDead = false;

    public void Setup(float health)
    {
        // Set entity stats
        this.health = health;
        isDead = false;

        // Select a crystal at random
        crystalData = crystals[Random.Range(0, crystals.Count)];
        crystalBorder.material = crystalData.color;
        deathMaterial = crystalData.color;
    }

    // Set the speed of the crystal
    public void SetSpeed()
    {
        rb.AddForce(new Vector2(Random.Range(-2000, 2000), Random.Range(-2000, 2000)));
        rb.angularVelocity = Random.Range(-1000, 1000);
    }

    // On collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the other enemy component
        Bullet bullet = collision.GetComponent<Bullet>();

        // If is enemy, damage
        if (bullet != null)
            bullet.OnHit(this);
    }

    // On collision
    public override void Damage(float amount, float knockback = -10f)
    {
        Damage(amount, knockback, EnemyHandler.active.player.position);
    }

    // On collision
    public void Damage(float amount, float knockback, Vector3 origin)
    {
        // Add knockback
        Knockback(knockback / 2, origin);

        // Calculate health
        health -= amount;
        if (health <= 0)
            Destroy();
        else AudioPlayer.Play(crystalSound);
    }

    // Knockback entity
    public override void Knockback(float amount, Vector3 origin)
    {
        // Apply knockback
        rb.AddForce(Vector3.Normalize(origin - transform.position) * amount);
    }

    // On destroy
    public override void Destroy()
    {
        // Check if already dead
        if (isDead) return;
        
        // Depending on crystal, do stuff
        switch (crystalData.type)
        {
            // If blue crystal, drop XP
            case CrystalType.blue:
                XPHandler.active.Spawn(transform.position, Random.Range(50, 100));
                break;

            // If green crystal, heal
            case CrystalType.green:
                Deck.player.Heal(10f);
                break;

            // If red crystal, heal
            case CrystalType.red:
                Events.active.BloodCrystalBroken();
                break;
        }

        // Add crystal to save data
        SaveSystem.AddCrystal(crystalData.InternalID, 1);

        // Set is dead flag
        isDead = true;

        // Destroy the crystal
        AudioPlayer.Play(crystalDestroy);
        Destroy(gameObject);
    }
}
