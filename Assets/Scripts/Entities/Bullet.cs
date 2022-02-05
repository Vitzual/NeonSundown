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

    // Set up the bullet
    public virtual void Setup(Weapon parent, WeaponData weapon, Transform target = null)
    {
        // Check if prestiged
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
            sprite.material = weapon.material;
        if (weapon.useTrail)
            trail.material = weapon.trail;

        // Set death materials and effect
        deathMaterial = weapon.material;
        deathEffect = weapon.particle;

        // Set bullet stats
        damage = parent.damage;
        speed = parent.moveSpeed;
        pierce = parent.pierces;
        tracking = weapon.trackTarget || Deck.GetFlag(Stat.Tracking);

        // Give bullets a bit of randomness
        float lowValue = parent.lifetime - 0.05f;
        float highValue = parent.lifetime + 0.05f;
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
        if (weapon.useParticle) CreateParticle();
        Destroy(gameObject);
    }

    // On collision
    public void OnHit(Entity entity)
    {
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
        entity.Damage(damage);

        // Check if bullet has a sound
        if (weapon.onDamageSound != null)
            AudioPlayer.Play(weapon.onDamageSound, true, weapon.minPitch, weapon.maxPitch);

        // Check if bullet has a sound
        if (weapon.onDeathSound != null && entity.IsDead())
            AudioPlayer.Play(weapon.onDeathSound, true, weapon.minPitch, weapon.maxPitch);

        if (pierce <= 0)
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

    // Get the damage
    public float GetDamage() { return damage; }
}
