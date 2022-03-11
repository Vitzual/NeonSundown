using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines : Secondary
{
    // Mine instance
    public Bullet mine;
    public Material material;
    public float damage;
    public float knockback;

    // Virtual use method
    public override void Use()
    {
        if (cooldown <= 0)
        {
            // Reset cooldown
            cooldown = data.cooldown;

            // Create the tile
            Bullet newMine = Instantiate(mine, transform.position, Quaternion.identity);
            newMine.explosive = true;
            newMine.isSplitShot = true;
            newMine.deathMaterial = material;
            newMine.SetDamage(damage);
            newMine.SetKnockback(knockback);

            // Play sound
            AudioPlayer.Play(sound);
        }
    }
}
