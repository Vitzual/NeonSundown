using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    // Weapon data
    protected WeaponData weapon;

    // Prestige object
    public GameObject baseModel;
    public GameObject prestigeModel;

    // Bullet components 
    public SpriteRenderer sprite;
    public TrailRenderer trail;
    public Transform rotator;

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
    public bool explosive;
    public bool stickyImmune;
    public bool stunEnemies;

    // Is a split shot
    private Weapon parent;
    [HideInInspector]
    public bool isSplitShot = false;
    private List<Transform> sticks = new List<Transform>();

    // Set up the bullet
    public virtual void Setup(Weapon parent, WeaponData weapon, Material material, 
        Transform target = null, bool isSplitShot = false, bool explosiveRound = false)
    {
        // Check if prestiged
        this.parent = parent;
        if (parent.prestige && prestigeModel != null)
            prestigeModel.SetActive(true);

        // Set primary weapon SO
        this.weapon = weapon;

        // Set target (if there is one)
        if (weapon.randomTarget)
        {
            Enemy enemy = EnemyHandler.active.GetRandomEnemy();
            if (enemy != null) this.target = enemy.transform;
        }
        else this.target = target;

        // Set renderer componenets
        if (weapon.useMaterial)
            sprite.material = material;
        if (weapon.useTrail)
            trail.material = weapon.trail;

        // Set death materials and effect
        deathMaterial = material;
        deathEffect = weapon.particle;

        // Set bullet stats
        damage = parent.damage;
        speed = parent.moveSpeed;
        pierce = parent.pierces;
        knockback = parent.knockback;
        tracking = weapon.trackTarget || Deck.GetFlag(Stat.Tracking);
        splitshots = parent.splitshots;
        if (!explosive) explosive = parent.explosiveRounds;

        // Give bullets a bit of randomness
        float lowValue = parent.lifetime - 0.05f;
        float highValue = parent.lifetime + 0.05f;
        if (lowValue <= 0f) lowValue = 0.001f;
        lifetime = Random.Range(lowValue, highValue);

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
            else RotateToTarget(weapon.lockTarget);
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
        // Check if sticky
        if (BulletHandler.stickyBullets && !stickyImmune && !isSplitShot)
        {
            foreach (Transform child in sticks)
                if (child != null) child.SetParent(null);
        }

        // Check if bullet is explosive
        if (explosive)
        {
            if (stunEnemies) ExplosiveHandler.CreateStun(transform.position, 10f, stunLength, deathMaterial, knockback);
            else ExplosiveHandler.CreateExplosion(transform.position, 10f, damage, knockback, deathMaterial);
        }
        
        // Check if bullet has splitshots
        if (!isSplitShot) BulletHandler.active.CreateSplitshot(parent, weapon, transform.position,
            transform.rotation, (int)splitshots, weapon.material, 360f, explosive);

        // Destroy the bullet
        if (!explosive && weapon.useParticle) CreateParticle();
        Destroy(gameObject);
    }

    // On collision
    public void OnHit(Entity entity)
    {
        // Check if sticky
        if (BulletHandler.stickyBullets && !stickyImmune && !isSplitShot)
        {
            //AudioPlayer.PlayStickySound();
            entity.transform.SetParent(transform);
            sticks.Add(entity.transform);
            return;
        }

        // Check if explosive
        if (explosive)
        {
            Destroy();
            return;
        }

        // Get material to hold
        Material holder = entity.GetMaterial();

        // Check if entity overrides this particle
        if (entity.overrideOtherParticles)
        {
            deathMaterial = holder;
            deathEffect = entity.deathEffect;
        }

        // Remove pierces
        pierce -= 1;
        entity.Damage(damage, knockback);

        // Check if entity dead
        if (!entity.IsDead() && stunEnemies)
            entity.Stun(stunLength);

        // Check if bullet has a sound
        if (weapon.onDamageSound != null)
            AudioPlayer.Play(weapon.onDamageSound, true, weapon.minPitch, weapon.maxPitch);

        // Check if bullet has a sound
        if (weapon.onDeathSound != null && entity.IsDead())
            AudioPlayer.Play(weapon.onDeathSound, true, weapon.minPitch, weapon.maxPitch);

        // Check pierce amount
        if (pierce < 0)
        {
            deathMaterial = holder;
            Destroy();
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
}
