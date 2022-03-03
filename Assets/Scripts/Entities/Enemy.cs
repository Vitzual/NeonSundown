using System.Collections;
using System.Collections.Generic;
using Thinksquirrel.CShake;
using UnityEngine;

public class Enemy : Entity
{
    // Scriptable object
    protected Variant variant;
    protected VariantData data;
    protected Material material;

    // Rigidbody attached to the enemy
    public Rigidbody2D rb;
    public bool isCullable = true;

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
    public virtual void Setup(VariantData data, Transform player)
    {
        // Get the variant color
        VariantColor variantColor = VariantPalette.GetVariantColor(data.variant);

        // Loop through all glows and fills
        foreach (TrailRenderer trail in trails) 
            trail.material = variantColor.material;
        foreach (SpriteRenderer glow in glows)
            glow.material = variantColor.material;
        foreach (SpriteRenderer fill in fills)
            fill.color = variantColor.fillColor;

        // Set scriptable
        variant = data.variant;
        this.data = data;

        // Set material / particle
        deathEffect = variantColor.deathParticle;
        deathMaterial = variantColor.material;
        material = variantColor.material;

        // Set stats
        health = data.health;
        maxHealth = health;

        // Set target
        target = player;
    }

    // Damage entity
    public override void Damage(float amount)
    {
        // Apply knockback
        rb.AddForce((target.position - transform.position) * -10);

        // Modify internal values
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
        
        // Check if enemy can shake screen on death
        if (data.shakeScreenOnDeath && Settings.screenShake)
            CameraShake.ShakeAll();

        // Destroy the object
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
            // Check if target should be locked
            if (data.lockTarget)
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
            else
            {
                // Gradually rotate to the target
                float rotateStep = data.rotateSpeed * Time.deltaTime;
                float angle = Mathf.Atan2(target.transform.position.y - transform.position.y, 
                    target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateStep);

                // Move towards the target
                float movementStep = data.speed * Time.deltaTime;
                transform.position += transform.up * movementStep;
            }
        }
    }

    // Get material function
    public override Material GetMaterial()
    {
        return material;
    }

    // Check if enemy is dead
    public override bool IsDead()
    {
        return health <= 0;
    }

    // Get enemy data
    public VariantData GetData() { return data; }

    // Get health variables
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }

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
