using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingTotem : BaseTotem
{
    // Healing variables
    public float healAmount;
    public float healCooldown;

    // On start, play animation
    public override void CustomUpdate()
    {
        if (Dealer.isOpen) return;

        if (radius.localScale.x < targetRadius)
            radius.localScale += scaleSpeed * Time.deltaTime;

        if (cooldown <= 0)
        {
            if (shipInsideRange)
            {
                Ship.Heal(healAmount);
                cooldown = healCooldown;
            }
        }
        else cooldown -= Time.deltaTime;
    }

    // Overrides the get stat function
    public override float GetStat(Stat stat)
    {
        if (stat == Stat.Cooldown)
            return healCooldown;
        else return -1;
    }
}
