using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Entity
{
    // Internal values
    public Rigidbody2D rb;
    public AudioClip crystalSound;
    public AudioClip crystalDestroy;
    private int minAmount = 25;
    private int maxAmount = 50;
    private float health = 50;

    public void Setup(float health, int min, int max)
    {
        this.health = health;
        minAmount = min;
        maxAmount = max;
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
    public override void Damage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Destroy();
        else AudioPlayer.Play(crystalSound);
    }

    // On destroy
    public override void Destroy()
    {
        AudioPlayer.Play(crystalDestroy);
        XPHandler.active.Spawn(transform.position, Random.Range(minAmount, maxAmount));
        Destroy(gameObject);
    }
}
