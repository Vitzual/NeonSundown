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

    // Upgrade mines
    public override void Upgrade()
    {
        // Apply new effects
        if (level < data.levels.Count)
        {
            StatValue stat = data.levels[level].stat;
            switch (stat.type)
            {
                case Stat.Damage:
                    if (stat.multiply) damage *= stat.modifier;
                    else damage += stat.modifier;
                    break;
                case Stat.Knockback:
                    if (stat.multiply) knockback *= stat.modifier;
                    else knockback += stat.modifier;
                    break;
            }
        }

        // Increase level
        base.Upgrade();
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
