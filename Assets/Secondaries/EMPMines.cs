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
        if (cooldown <= 0 && !Dealer.isOpen)
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

    // Overrides the get stat function
    public override float GetStat(Stat stat)
    {
        if (stat == Stat.StunLength)
            return stunTime;
        else return base.GetStat(stat);
    }
}
