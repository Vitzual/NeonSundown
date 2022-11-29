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
    public Transform rotator;
    public GameObject autoLockRange;

    // The bullets target
    protected Transform target;
    
    // Active variables
    protected float damage;
    protected float speed;
    protected float pierce;
    protected bool tracking;
    protected float lifetime;
    protected float knockback;
    protected float splitshots;
    protected float bulletSize;
    protected float richochets;
    public bool explosive;
    public bool overrideSprite;
    public float explosionSize = 10f;
    public bool chainTarget = false;
    public float castRange = 0;
    private bool isReversed = false;
    public bool canPassBarriers = false;
    private bool autoLock = false;
    private bool lockTarget = false;
    private bool informOnHit = false;

    // Is a split shot
    protected Weapon parent;
    [HideInInspector]
    public bool isSplitShot = false;
    protected Material normalMaterial;

    // Set up the bullet
    public virtual void Setup(Weapon parent, WeaponData weapon, Material material, Transform target = null, 
        bool isSplitShot = false, bool explosiveRound = false, bool autoLock = false)
    {
        // Check if auto lock being used
        if (autoLock && autoLockRange != null)
        {
            this.autoLock = true;
            lockTarget = true;
            autoLockRange.SetActive(true);
        }
        else lockTarget = weapon.lockTarget;

        // Check if prestiged
        this.parent = parent;
        this.weapon = weapon;

        // Set target (if there is one)
        if (weapon.randomTarget)
        {
            Enemy enemy = EnemyHandler.active.GetRandomEnemy();
            if (enemy != null) this.target = enemy.transform;
        }
        else this.target = target;

        // Set renderer componenets
        if (!overrideSprite)
        {
            if (weapon.useMaterial)
                sprite.material = material;
            if (weapon.useTrail)
                trail.material = weapon.trail;
        }

        // Set death materials and effect
        deathMaterial = material;
        normalMaterial = material;
        deathEffect = weapon.particle;

        // Set bullet stats
        if (parent.speedAffectsDamage)
        {
            if (Controller.horizontal == 0 && Controller.vertical == 0) damage = 0;
            else damage = parent.damage * (parent.speedDamageMultiplier * Controller.speed) * parent.damageMultiplier;
        }
        else damage = parent.damage * parent.damageMultiplier;

        // Set other variable stats
        speed = parent.moveSpeed;
        pierce = parent.pierces;
        knockback = parent.knockback;
        tracking = weapon.trackTarget || Deck.GetFlag(Stat.Tracking);
        splitshots = parent.splitshots;
        stunLength = parent.stunLength;
        bulletSize = parent.size;
        informOnHit = parent.informOnHit;
        richochets = parent.richochets;
        if (!explosive) explosive = parent.explosiveRounds;

        // Give bullets a bit of randomness
        float lowValue = parent.lifetime - 0.05f;
        float highValue = parent.lifetime + 0.05f;
        if (lowValue <= 0f) lowValue = 0.001f;
        lifetime = Random.Range(lowValue, highValue);

        // If splitshot, divide health
        if (isSplitShot) lifetime /= 2;

        // Check if splitshot
        this.isSplitShot = isSplitShot;
    }

    // Moves the bullet
    public virtual void Move()
    {
        // Decay bullet
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) Destroy();

        // Check if tracking
        if (tracking)
        {
            // Check if target is null
            if (target == null) tracking = false;

            // Rotate randomly and move forward
            else RotateToTarget(lockTarget);
        }
        
        // Move forward
        transform.position += transform.up * speed * Time.deltaTime;
        
        // Check if spinning
        if (weapon.rotate)
            rotator.Rotate(Vector3.forward, weapon.rotateSpeed * Time.deltaTime);
    }

    // Destroy the bullet
    public override void Destroy()
    {      
        // Check if bullet is explosive
        if (explosive)
        {
            if (stunLength > 0) ExplosiveHandler.CreateStun(transform.position, explosionSize,
                stunLength, damage, deathMaterial, knockback, parent);
            else ExplosiveHandler.CreateExplosion(transform.position, explosionSize, damage,
                knockback, deathMaterial, parent);
        }
        
        // Check if bullet has splitshots
        if (!isSplitShot) BulletHandler.active.CreateSplitshot(parent, weapon, transform.position,
            transform.rotation, bulletSize, (int)splitshots, normalMaterial, 360f, explosive);

        // Destroy the bullet
        if (!explosive && weapon.useParticle) CreateParticle();
        Destroy(gameObject);
    }

    // On collision
    public virtual void OnHit(Entity entity)
    {
        // Check if entity is dead
        if (entity.IsDead()) return;

        // Check if locked
        if (autoLock)
        {
            autoLockRange.SetActive(false);
            target = entity.transform;
            tracking = true;
            autoLock = false;
        }
        else
        {
            // Remove pierces
            pierce -= 1;
            entity.Damage(damage, knockback);

            // Check if entity dead
            if (!entity.IsDead()) 
            {
                if (stunLength > 0) entity.Stun(stunLength);
                if (informOnHit) parent.TargetHit(entity);
            }

            // Check if bullet has a sound
            if (weapon != null && weapon.onDamageSound != null)
                AudioPlayer.Play(weapon.onDamageSound, true, weapon.minPitch, weapon.maxPitch);

            // Check if bullet has a sound
            if (weapon != null && weapon.onDeathSound != null && entity.IsDead())
                AudioPlayer.Play(weapon.onDeathSound, true, weapon.minPitch, weapon.maxPitch);

            // Check for richochets
            if (richochets > 0)
            {
                richochets -= 1;
                ReverseBullet(false);
            }

            // Check pierce amount
            else if (pierce < 0)
            {
                // Get material to hold
                Material holder = entity.GetMaterial();

                // Check if entity overrides this particle
                if (entity.overrideOtherParticles)
                {
                    deathMaterial = holder;
                    deathEffect = entity.deathEffect;
                }

                deathMaterial = holder;
                Destroy();
            }

            // If not dead, chain target
            else if (chainTarget) FindChainTarget(castRange);
        }
    }

    // Rotate to target
    public virtual void RotateToTarget(bool lockTarget)
    {
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
            target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        if (lockTarget) transform.rotation = targetRotation;
        else transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            targetRotation, weapon.rotateSpeed * Time.deltaTime);
    }

    // Set the damage
    public void SetDamage(float amount) { damage = amount; }
    public void SetKnockback(float amount) { knockback = amount; }
    public void SetStunLength(float amount) { stunLength = amount; }

    // Get the damage
    public float GetDamage() { return damage; }

    // Reverse bullet
    public void ReverseBullet(bool playSound = true)
    {
        if (!isReversed)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 180f);
            if (playSound) AudioPlayer.PlayReflectSound();
            isReversed = true;
        }
    }

    // Find chain target
    public void FindChainTarget(float castRange)
    {
        // Set target to null and cast for entities
        Collider2D[] colliders = ExplosiveHandler.CastForEntities(transform.position, castRange);

        // Loop through colliders to find new target
        for (int i = 0; i < colliders.Length; i++)
        {
            Transform newTarget = colliders[i].GetComponent<Transform>();
            if (newTarget != target)
            {
                target = newTarget;
                tracking = true;
                return;
            }
        }

        // If no target found, set target to null
        target = null;
        tracking = false;
    }
}
