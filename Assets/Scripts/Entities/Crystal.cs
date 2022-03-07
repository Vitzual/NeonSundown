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

    public void Setup(float health)
    {
        // Set entity stats
        this.health = health;

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
        rb.AddForce(Vector3.Normalize(origin - transform.position) * (knockback / 2));

        // Calculate health
        health -= amount;
        if (health <= 0)
            Destroy();
        else
        {
            // Do something based on crystal
            AudioPlayer.Play(crystalSound);
            switch (crystalData.type)
            {
                // If blue crystal, drop XP
                case CrystalType.blue:
                    XPHandler.active.Spawn(transform.position, Random.Range(1, 3));
                    break;
            }
        }
    }

    // On destroy
    public override void Destroy()
    {
        // Add crystal to save data
        if (SaveSystem.saveData != null)
        {
            if (SaveSystem.saveData.crystals.ContainsKey(crystalData.InternalID))
                SaveSystem.saveData.crystals[crystalData.InternalID] += 1;
            else SaveSystem.saveData.crystals.Add(crystalData.InternalID, 1);
        }

        // Destroy the crystal
        AudioPlayer.Play(crystalDestroy);
        Destroy(gameObject);
    }
}
