using System.Collections;
using System.Collections.Generic;
using Thinksquirrel.CShake;
using UnityEngine;

public class Enemy : Entity
{
    // Scriptable object
    protected Variant variant;
    protected VariantData variantData;
    protected EnemyData enemyData;
    protected Material material;

    // Rigidbody attached to the enemy
    public Rigidbody2D rb;
    public bool isCullable = true;
    private bool isDestroyed = false;
    private bool lockOn = false;
    public bool isBoss = false;
    public bool isTameable = true;
    public bool isClone = false;
    public bool dieOnCollision = true;

    // Transform lists
    public List<TrailRenderer> trails;
    public List<SpriteRenderer> glows;
    public List<SpriteRenderer> fills;
    public List<ParticleSystemRenderer> particles;
    public Transform rotator;

    // Internal runtime variables
    private float health;
    private float maxHealth;
    [HideInInspector] public float moveSpeed;
    public bool isSlowed = false;
    protected float rotateSpeed;
    private float damage;
    private bool seeded = false;
    
    // Runtime only variables
    private float rotateStep, angle, step, dmg, stunImmunity;

    // Target transform for moving
    protected Transform target;
    protected bool hasRigidbody;
    
    // Setup the enemy
    public virtual void Setup(EnemyData data, Variant variant, Transform target)
    {
        // Set scriptable
        enemyData = data;
        this.variant = variant;
        variantData = enemyData.variants[variant];

        // Get the variant color
        VariantColor variantColor = VariantPalette.GetVariantColor(variant);

        // Loop through all glows and fills
        foreach (TrailRenderer trail in trails) 
            trail.material = variantColor.material;
        foreach (SpriteRenderer glow in glows)
            glow.material = variantColor.material;
        foreach (SpriteRenderer fill in fills)
            fill.color = variantColor.fillColor;

        // Loop through particle trails and sprites
        foreach (ParticleSystemRenderer particle in particles)
        {
            particle.material = variantColor.material;
            particle.trailMaterial = variantColor.material;
        }

        // Set material / particle
        deathEffect = variantColor.deathParticle;
        deathMaterial = variantColor.material;
        material = variantColor.material;

        // Set stats
        health = variantData.health * ArenaController.enemyHealthMultiplier;
        moveSpeed = variantData.speed * ArenaController.enemySpeedMultiplier;
        damage = variantData.damage * ArenaController.enemyDamageMultiplier;
        maxHealth = health;
        rotateSpeed = variantData.rotateSpeed;
        immune = variantData.immune;

        // Set target
        this.target = target;

        // Set rigidbody flag
        hasRigidbody = rb != null;
    }

    // Damage entity
    public override void Damage(float amount, float knockback = -10f, bool overrideImmunity = false)
    {
        if (target != null) Damage(amount, knockback, target.position);
        else Damage(amount, knockback, Vector3.zero);
    }

    // Damage entity
    public void Damage(float amount, float knockback, Vector3 origin, bool overrideImmunity = false)
    {
        // Check if immune
        if (!overrideImmunity)
        {
            if (immune)
            {
                DamageHandler.active.CreateImmune(transform.position);
                return;
            }
            else if (seeded) return;
        }
        
        // Add to total damage done
        RuntimeStats.damageGiven += amount;

        // Do the damage my guy
        if (Random.Range(0f, 1f) < DamageHandler.critChance)
        {
            // Apply double damage and knockback
            dmg = amount * 2f;
            health -= dmg;
            Knockback(knockback * 1.5f, origin);
            DamageHandler.active.CreateNumber(transform.position, dmg, true);
            AudioPlayer.PlayCritSound();

            // Check if enemy can shake screen
            if (Settings.screenShake) CameraShake.ShakeAll();
        }
        else
        {
            // Apply normal damage
            health -= amount;
            Knockback(knockback, origin);
            DamageHandler.active.CreateNumber(transform.position, amount, false);
        }
        if (IsDead()) Destroy();
    }

    // Knockback entity
    public override void Knockback(float amount, Vector3 origin, bool forceKnockback = false)
    {
        // Apply knockback
        if (hasRigidbody && (variantData.knockback || forceKnockback))
            rb.AddForce(Vector3.Normalize(origin - transform.position) * amount);
    }

    // Destroy entity
    public override void Destroy()
    {
        // Check if immune
        if (immune) return;

        // Check objective status
        if (!Gamemode.awardGiven &&
            Gamemode.arena.useAchievementKills &&
            Gamemode.arena.enemyToKill == enemyData)
        {
            Debug.Log("Awarding achievement!");
            Gamemode.ArenaObjectiveComplete();
        }

        // Check if boss
        if (isBoss) Events.active.BossDestroyed();

        // Create particle
        CreateParticle();

        // Spawn XP and possibly crystal
        if (!isDestroyed)
        {
            if (variantData.canDropCrystal)
            {
                if (Random.Range(0, 1f) < (variantData.crystalDropChance * ArenaController.crystalDropChance))
                    XPHandler.active.Spawn(transform.position, variantData.minXP, variantData.crystal);
                else XPHandler.active.Spawn(transform.position, variantData.minXP);
            }
            else XPHandler.active.Spawn(transform.position, variantData.minXP);
        }

        // Set is destroy to true
        isDestroyed = true;
        RuntimeStats.enemiesDestroyed += 1;

        // Possibly syphon
        if (EnemyHandler.syphon > 0)
            Ship.Heal(EnemyHandler.syphon);

        // Destroy the object
        Destroy(gameObject);
    }

    // Called when a player hits this enemy
    public virtual void OnHitPlayer(Ship player)
    {
        // Check if seeded
        if (seeded) return;

        if (immune) { player.Kill(); return; }
        player.Damage(damage);
        if (dieOnCollision) Destroy();
    }

    // Move towards the target
    public virtual void Move()
    {
        // Check if stunned
        if (stunned)
        {
            stunLength -= Time.deltaTime;
            if (stunLength <= 0f)
                stunned = false;
            return;
        }

        // Check if stun immunity is active
        if (stunImmunity > 0f) stunImmunity -= Time.deltaTime;

        // Move if target not null
        if (target != null)
        {
            // Calculate step
            step = moveSpeed * Time.deltaTime;

            if (variantData.rotate)
            {
                // Check if rotating (lock target)
                rotator.Rotate(Vector3.forward, variantData.rotateSpeed * Time.deltaTime);
                transform.position = Vector2.MoveTowards(transform.position, target.position, step);
            }

            else
            {
                // Rotate towards the object
                rotateStep = rotateSpeed * Time.deltaTime;
                angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
                    target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateStep);
                if (lockOn) transform.position = Vector2.MoveTowards(transform.position, target.position, step);
                else transform.position += transform.up * step;
            }
        }
        else if (EnemyHandler.active.player != null)
        {
            target = EnemyHandler.active.player;
        }
    }

    // Seeds the enemy
    public virtual void SeedEnemy(Ship ship)
    {
        // Check if boss
        if (isBoss || !isTameable) return;

        // Calculate fill color
        Color fillColor = new Color(ship.fill.color.r * 0.2f,
            ship.fill.color.g * 0.2f, ship.fill.color.b * 0.2f, 1f);
        Material borderMaterial = ship.border.material;
        
        // Change glow and fills
        foreach (SpriteRenderer sprite in glows) sprite.material = borderMaterial;
        foreach (SpriteRenderer sprite in fills) sprite.color = fillColor;
        foreach (TrailRenderer trail in trails) trail.material = borderMaterial;
        foreach (ParticleSystemRenderer particle in particles)
        {
            particle.material = borderMaterial;
            particle.trailMaterial = borderMaterial;
        }

        // Set seed to true
        seeded = true;
        target = ship.transform;
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

    // Set target
    public void SetTarget(Transform newTarget) { target = newTarget; }
    public Transform GetTarget() { return target; }

    // Get object data
    public Variant GetVariant() { return variant; }
    public EnemyData GetEnemyData() { return enemyData; }
    public VariantData GetVariantData() { return variantData; }


    // Get health variables
    public float GetHealth() { return health; }
    public float GetMaxHealth() { return maxHealth; }
    public bool LockOnEnabled() { return lockOn; }
    public bool RotationEnabled() { return !rb.freezeRotation; }

    // On collision
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the other enemy component
        Bullet bullet = collision.GetComponent<Bullet>();

        // If is bullet, invoke on hit method
        if (bullet != null) 
        {
            bullet.OnHit(this);
            return;
        }
        else if (collision.transform.parent != null)
        {
            // Get the other enemy component
            bullet = collision.transform.parent.GetComponent<Bullet>();

            // If is bullet, invoke on hit method
            if (bullet != null)
            {
                bullet.OnHit(this);
                return;
            }
        }
    }

    public override void Stun(float length)
    {
        if (stunImmunity <= 0f)
        {
            stunned = true;
            stunLength = length;
            stunImmunity = 1.5f;
        }
    }
    
    public void EnableLockOn()
    {
        lockOn = true;
    }

    public void DisableRotation()
    {
        if (hasRigidbody)
            rb.freezeRotation = true;
        rotateSpeed = 0f;
    }

    public bool IsSeeded()
    {
        return seeded;
    }
}
