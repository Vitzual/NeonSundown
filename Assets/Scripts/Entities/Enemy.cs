using System.Collections;
using System.Collections.Generic;
using Thinksquirrel.CShake;
using UnityEngine;

public class Enemy : Entity
{
    // Scriptable object
    protected Variant variant;
    protected VariantData data;

    // Transform lists
    public List<TrailRenderer> trails;
    public List<SpriteRenderer> glows;
    public List<SpriteRenderer> fills;
    public Transform rotator;

    // Internal runtime variables
    private float health;
    private float maxHealth;

    // Target transform for moving
    private Transform target;
    
    // Setup the enemy
    public virtual void Setup(VariantData data, Variant variant, Transform player)
    {
        // Loop through all glows and fills
        foreach (TrailRenderer trail in trails) 
            trail.material = data.material;
        foreach (SpriteRenderer glow in glows)
            glow.material = data.material;
        foreach (SpriteRenderer fill in fills)
            fill.color = data.color;

        // Set scriptable
        this.variant = variant;
        this.data = data;

        // Set material / particle
        deathEffect = data.deathParticle;
        deathMaterial = data.material;

        // Set stats
        health = data.health;
        maxHealth = health;

        // Set target
        target = player;
    }

    // Damage entity
    public override void Damage(float amount)
    {
        health -= amount;
        if (IsDead()) Destroy();
    }

    // Destroy entity
    public override void Destroy()
    {
        CreateParticle();

        // Spawn XP and possibly crystal
        if (data.canDropCrystal)
        {
            if (Random.Range(0, 1f) < data.crystalDropChance)
                XPHandler.active.Spawn(transform.position, data.minXP, data.crystal);
            else XPHandler.active.Spawn(transform.position, data.minXP);
        }
        else XPHandler.active.Spawn(transform.position, data.minXP);

        CameraShake.ShakeAll();
        Destroy(gameObject);
    }

    // Called when a player hits this enemy
    public virtual void OnHitPlayer(Ship player)
    {
        player.Damage(data.damage);
        Destroy();
    }

    // Move towards the target
    public virtual void Move()
    {
        if (target != null)
        {
            if (data.rotate)
            {
                // Rotate if it says to 
                rotator.Rotate(Vector3.forward, data.rotateSpeed * Time.deltaTime);
            }
            else
            {
                // Rotate to the target
                float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
                    target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
                transform.rotation = targetRotation;
            }

            // Move towards the target
            float step = data.speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
    }

    // Get material function
    public override Material GetMaterial()
    {
        return data.material;
    }

    // Check if enemy is dead
    public override bool IsDead()
    {
        return health <= 0;
    }

    // Get enemy data
    public VariantData GetData()
    {
        return data;
    }

    // On collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the other enemy component
        Bullet bullet = collision.GetComponent<Bullet>();

        // If is bullet, invoke on hit method
        if (bullet != null) 
        {
            bullet.OnHit(this);
            return;
        }
    }
}
