using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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

    // The particle effect for this bullet
    private Material deathMaterial;
    private ParticleSystem deathEffect;

    // Set up the bullet
    public virtual void Setup(WeaponData weapon)
    {
        // Set renderer componenets
        sprite.sprite = weapon.model;
        sprite.material = weapon.material;
        //trail.material = weapon.trail;
        deathMaterial = weapon.material;
        deathEffect = weapon.effect;

        // Set bullet stats
        damage = weapon.damage * Deck.GetMultiplier(Stat.Damage) + Deck.GetAdditions(Stat.Damage);
        speed = weapon.speed * Deck.GetMultiplier(Stat.Speed) + Deck.GetAdditions(Stat.Damage);
        pierce = weapon.pierces * Deck.GetMultiplier(Stat.Pierces) + Deck.GetAdditions(Stat.Damage);
        lifetime = weapon.lifetime * Deck.GetMultiplier(Stat.Lifetime) + Deck.GetAdditions(Stat.Damage);
        tracking = (weapon.tracking || Deck.GetFlag(Stat.Tracking)) && target != null;
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
        CreateParticle();
        Destroy(gameObject);
    }

    // Creates a particle and sets the material
    public void CreateParticle()
    {
        ParticleSystemRenderer holder = Instantiate(deathEffect, transform.position,
                transform.rotation).GetComponent<ParticleSystemRenderer>();
        holder.transform.rotation *= Quaternion.Euler(-90, 90, 0);
        holder.material = deathMaterial;
        holder.trailMaterial = deathMaterial;
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
