using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    // Bullet components 
    public SpriteRenderer sprite;
    public TrailRenderer trail;

    // The bullets target
    private Transform target;

    // Active variables
    private float damage;
    private float speed;
    private float pierce;
    private bool tracking;
    private float lifetime;

    // Set up the bullet
    public virtual void Setup(PrimaryData weapon)
    {
        // Set renderer componenets
        sprite.sprite = weapon.model;
        sprite.material = weapon.material;
        //trail.material = weapon.trail;
        deathMaterial = weapon.material;
        deathEffect = weapon.effect;

        // Set bullet stats
        damage = Deck.CalculateStat(Stat.Damage, weapon.damage);
        speed = Deck.CalculateStat(Stat.Speed, weapon.speed);
        pierce = Deck.CalculateStat(Stat.Pierces, weapon.pierces);
        tracking = (weapon.tracking || Deck.GetFlag(Stat.Tracking)) && target != null;

        // Give bullets a bit of randomness
        float lowValue = Deck.CalculateStat(Stat.Lifetime, weapon.lifetime) - 0.1f;
        float highValue = Deck.CalculateStat(Stat.Lifetime, weapon.lifetime) + 0.1f;
        if (lowValue <= 0f) lowValue = 0.001f;
        lifetime = Random.Range(lowValue, highValue);
    }

    // Moves the bullet
    public virtual void Move()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) Destroy();

        if (tracking && target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
        else transform.position += transform.up * speed * Time.deltaTime;
    }

    // Destroy the bullet
    public virtual void Destroy()
    {
        CreateParticle(true);
        Destroy(gameObject);
    }

    // On collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the other enemy component
        Enemy enemy = collision.GetComponent<Enemy>();

        // If is enemy, damage
        if (enemy != null)
        {
            // Get material to hold
            Material holder = enemy.GetMaterial();

            // Remove pierces
            pierce -= 1;
            enemy.Damage(damage);
            if (pierce <= 0)
            {
                deathMaterial = holder;
                Destroy();
            }
        }
    }
}
