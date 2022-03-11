using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPMines : Mines
{
    // Gets the stun time
    public float stunTime;

    // Virtual use method
    public override void Use()
    {
        if (cooldown <= 0)
        {
            // Reset cooldown
            cooldown = data.cooldown;

            // Create the tile
            Bullet newMine = Instantiate(mine, transform.position, Quaternion.identity);
            newMine.isSplitShot = true;
            newMine.SetDamage(damage);
            newMine.SetKnockback(knockback);
            newMine.SetStunLength(stunTime);

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
            if (stat.type == Stat.Stun)
            {
                if (stat.multiply) stunTime *= stat.modifier;
                else stunTime += stat.modifier;
                return;
            }
            else base.Upgrade();
        }
    }

    // Overrides the get stat function
    public override float GetStat(Stat stat)
    {
        if (stat == Stat.Stun)
            return stunTime;
        else return base.GetStat(stat);
    }
}
