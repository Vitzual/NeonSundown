using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : Weapon
{
    // Secondary scythe
    public Scythe secondary;

    // Rotator transform
    public Transform rotator;
    public float xOffset = 0;
    public float yOffset = 0;

    // Override default setup
    public override void Setup(WeaponData data, Transform target = null)
    {
        transform.localPosition = new Vector2(transform.localPosition.x + xOffset, transform.localPosition.y + yOffset);
        base.Setup(data, target);
    }

    // Rotates around the player
    public override void Use()
    {
        rotator.Rotate(Vector3.forward, weapon.rotateSpeed * Time.deltaTime);
        transform.RotateAround(target.position, Vector3.forward, weapon.moveSpeed * Time.deltaTime);
    }

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            // Attempt to damage the enemy
            enemy.Damage(Player.CalculateStat(Stat.Damage, weapon.damage));

            // Play death sound if enemy dies
            if (enemy.IsDead()) AudioPlayer.Play(weapon.onDeathSound);
            else AudioPlayer.Play(weapon.onDamageSound);
        }
    }

    // Override upgrade method
    public override void Upgrade()
    {
        
    }
}
