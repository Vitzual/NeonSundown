using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageTotem : BaseTotem
{
    // Healing variables
    public float damageMultiplier;

    // Upgrade (literally just for cooldown)
    public override void Upgrade()
    {
        // Apply new effects
        if (level < data.levels.Count)
        {
            StatValue stat = data.levels[level].stat;
            if (stat.type == Stat.Damage)
            {
                if (stat.multiply) damageMultiplier *= stat.modifier;
                else damageMultiplier += stat.modifier;
                return;
            }
            else base.Upgrade();
        }
    }

    // Overrides the get stat function
    public override float GetStat(Stat stat)
    {
        if (stat == Stat.Cooldown)
            return cooldown;
        else if (stat == Stat.Damage)
            return damageMultiplier;
        else return -1;
    }

    // On collision with enemy, apply damage
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        base.OnTriggerEnter2D(collision);

        // Check if collided with ship
        Ship ship = collision.GetComponent<Ship>();
        if (ship != null) ship.damageMultiplier += damageMultiplier;
    }

    // On collision with enemy, apply damage
    public override void OnTriggerExit2D(Collider2D collision)
    {
        // Get the enemy component
        base.OnTriggerExit2D(collision);

        // Check if collided with ship
        Ship ship = collision.GetComponent<Ship>();
        if (ship != null) ship.damageMultiplier -= damageMultiplier;
    }
}
