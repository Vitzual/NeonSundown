using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    // Laser descrease speed
    public float laserSpeed;
    public bool enemyLaser;

    public void SetupLaser(Transform parent, float width, float length, float bloom)
    {
        // Offset rotation slightly
        Vector3 rotationOffset = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            transform.eulerAngles.z + Random.Range(-bloom, bloom));
        transform.eulerAngles = rotationOffset;

        // Set rotation
        transform.SetParent(parent.transform);
        transform.localPosition = Vector3.zero;
        transform.rotation = parent.transform.rotation;
        transform.localScale = new Vector2(width, length);
        transform.localPosition = new Vector2(0, length / 2);
    }

    // Lower laser size
    public override void Move()
    {
        if (transform.localScale.x > 0.2f)
        {
            transform.localScale = new Vector2(transform.localScale.x -
                (Time.deltaTime * laserSpeed), transform.localScale.y);
        }
        else Destroy();
    }

    // On hit, apply damage
    public override void OnHit(Entity entity)
    {
        // Check if enemy laser
        if (enemyLaser) return;

        // Remove pierces
        entity.Damage(damage, knockback);

        // Check if entity dead
        if (!entity.IsDead() && stunLength > 0)
            entity.Stun(stunLength);

        // Check if bullet is explosive
        if (explosive)
        {
            if (stunLength > 0) ExplosiveHandler.CreateStun(entity.transform.position, explosionSize, stunLength, damage, deathMaterial, knockback);
            else ExplosiveHandler.CreateExplosion(entity.transform.position, explosionSize, damage, knockback, deathMaterial);
        }
    }

    // On thing
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if enemy laser
        if (enemyLaser)
        {
            Ship ship = collision.GetComponent<Ship>();
            if (ship != null) ship.Damage(5f);
        }
    }

    // Override destroy
    public override void Destroy()
    {
        // Check if enemy laser
        if (enemyLaser) return;

        Destroy(gameObject);
    }
}