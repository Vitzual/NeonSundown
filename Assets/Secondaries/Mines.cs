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
        if (cooldown <= 0 && !Dealer.isOpen)
        {
            // Reset cooldown
            cooldown = data.cooldown;

            // Create the tile
            Bullet newMine = Instantiate(mine, transform.position, Quaternion.identity);
            newMine.isSplitShot = true;
            newMine.SetDamage(damage);
            newMine.SetKnockback(knockback);

            // Play sound
            AudioPlayer.Play(sound);
        }
    }

    // Overrides the get stat function
    public override float GetStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.Damage:
                return damage;
            case Stat.Knockback:
                return knockback;
            default:
                return base.GetStat(stat);
        }
    }
}
