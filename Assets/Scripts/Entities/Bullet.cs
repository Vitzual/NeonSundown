using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    // Weapon data
    protected WeaponData weapon;

    // Bullet components 
    public SpriteRenderer sprite;
    public TrailRenderer trail;

    // The bullets target
    protected Transform target;

    // Active variables
    protected float damage;
    protected float speed;
    protected float pierce;
    protected bool tracking;
    protected float lifetime;

    // Set up the bullet
    public virtual void Setup(WeaponData weapon, Transform target = null)
    {
        // Set primary weapon SO
        this.weapon = weapon;

        // Set target (if there is one)
        if (weapon.randomTarget)
        {
            Enemy enemy = EnemyHandler.active.GetRandomEnemy();
            if (enemy != null) target = enemy.transform;
        }
        else this.target = target;

        // Set renderer componenets
        if (weapon.useSprite)
            sprite.sprite = weapon.sprite;
        if (weapon.useMaterial)
            sprite.material = weapon.material;
        if (weapon.useTrail)
            trail.material = weapon.trail;

        // Set death materials and effect
        deathMaterial = weapon.material;
        deathEffect = weapon.effect;

        // Set bullet stats
        damage = Deck.CalculateStat(Stat.Damage, weapon.damage);
        speed = Deck.CalculateStat(Stat.Speed, weapon.moveSpeed);
        pierce = Deck.CalculateStat(Stat.Pierces, weapon.pierces);
        tracking = (weapon.trackTarget || Deck.GetFlag(Stat.Tracking)) && target != null;

        // Give bullets a bit of randomness
        float lowValue = Deck.CalculateStat(Stat.Lifetime, weapon.lifetime) - 0.1f;
        float highValue = Deck.CalculateStat(Stat.Lifetime, weapon.lifetime) + 0.1f;
        if (lowValue <= 0f) lowValue = 0.001f;
        lifetime = Random.Range(lowValue, highValue);
    }

    // Moves the bullet
    public virtual void Move()
    {
        // Decay bullet
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) Destroy();

        // Check if tracking
        if (tracking && target != null)
        {
            // Rotate randomly and move forward
            RotateToTarget(weapon.lockTarget);
        }
        
        // Move forward
        transform.position += transform.up * speed * Time.deltaTime;
    }

    // Destroy the bullet
    public virtual void Destroy()
    {
        CreateParticle();
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

    // Rotate to target
    public virtual void RotateToTarget(bool gradual)
    {
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
            target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        if (gradual) Quaternion.RotateTowards(transform.rotation, targetRotation, weapon.rotateSpeed * Time.deltaTime);
        else transform.rotation = targetRotation;
    }
}
