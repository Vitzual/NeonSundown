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

    // Upgrade (literally just for cooldown)
    public override void Upgrade()
    {
        // Apply new effects
        if (level < data.levels.Count)
        {
            StatValue stat = data.levels[level].stat;
            if (stat.type == Stat.Cooldown)
            {
                if (stat.multiply) healCooldown *= stat.modifier;
                else healCooldown += stat.modifier;
                return;
            }
            else base.Upgrade();
        }
    }

    // Overrides the get stat function
    public override float GetStat(Stat stat)
    {
        if (stat == Stat.Cooldown)
            return healCooldown;
        else return -1;
    }
}
