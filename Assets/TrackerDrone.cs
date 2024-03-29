using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerDrone : Drone
{
    // Reference to rigidBody
    public Rigidbody2D rb;

    // Runtime references
    public Transform target;
    private float cooldown = 0f;
    public AudioClip hitSound;

    // Set move speed
    public override void Setup(Ship ship, HelperData data)
    {
        target = ship.transform;
        base.Setup(ship, data);
        UpdateStat(Stat.Damage);
    }

    // Move drone towards target
    public override void Move()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        // Check if target is null
        if (target == null) target = ship.transform;

        // Calculate angle to target
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
            target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

        // Rotate to target
        transform.rotation = Quaternion.RotateTowards(transform.rotation,
            targetRotation, rotationSpeed * Time.deltaTime);

        // Move forward
        transform.position += transform.up * movementSpeed * Time.fixedDeltaTime;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Get enemy component
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        // Check if enemy is not null
        if (enemy != null)
        {
            //cooldown = 0.5f;
            enemy.Damage(damage, knockback);
            Knockback(-1500f, enemy.transform.position);
            if (hitSound != null) AudioPlayer.Play(hitSound);
        }
        else
        {
            // Check if crystal
            Crystal crystal = collision.collider.GetComponent<Crystal>();

            // Check if crystal is not null, and destroy
            if (crystal != null) crystal.Destroy();
        }
    }

    // Knockback entity
    public override void Knockback(float amount, Vector3 origin, bool forceKnockback = false)
    {
        // Apply knockback
        rb.AddForce(Vector3.Normalize(origin - transform.position) * amount);
    }

    // Set taregt
    public override void SetTarget(Entity entity)
    {
        target = entity.transform;
    }

    public override void UpdateStat(Stat type)
    {
        switch (type)
        {
            case Stat.Damage:
                damage = (Deck.CalculateStat(type, data.statAmount)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;

            default:
                base.UpdateStat(type);
                break;
        }
    }
}
