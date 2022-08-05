using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : Weapon
{
    // Rotator transform
    public Transform rotator;
    public float xOffset = 0;
    public float yOffset = 0;
    public bool heal = false;
    public Material material;

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
        transform.RotateAround(target.position, Vector3.forward, moveSpeed * Time.deltaTime);
    }

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Entity entity = collision.GetComponent<Entity>();
        if (entity != null)
        {
            // Attempt to damage the enemy
            if (explosiveRounds)
            {
                if (stunLength > 0f) ExplosiveHandler.CreateStun(entity.transform.position,
                    range, stunLength, damage, material, knockback);
                else ExplosiveHandler.CreateExplosion(entity.transform.position, range,
                    damage, knockback, material);
            }
            else
            {
                if (stunLength > 0f) entity.Stun(stunLength);
                entity.Damage(damage, weapon.knockback);
            }

            // Check if healing enabled
            if (heal) Ship.Heal(0.01f);

            // Play death sound if enemy dies
            if (entity.IsDead()) AudioPlayer.Play(weapon.onDeathSound, true, 0.9f, 1.1f, false, weapon.audioScale);
            else AudioPlayer.Play(weapon.onDamageSound);
        }
    }

    // Update stat
    public override void UpdateStat(Stat stat)
    {
        switch (stat)
        {
            // Increase explosive rounds
            case Stat.Explosive:
                explosiveRounds = Deck.GetAdditions(stat) > 2;
                break;

            default:
                base.UpdateStat(stat);
                break;
        }
    }
}
