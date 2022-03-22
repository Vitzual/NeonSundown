using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : Drone
{
    public AudioClip healSound;

    public float healAmount;
    public float healCooldown;

    public Transform radius;
    public float targetRadius;
    public Vector3 scaleSpeed;
    public float maxDistance;
    private float cooldown;
    private bool isHealing;

    // On start, play animation
    public override void CustomUpdate()
    {
        if (Dealer.isOpen) return;

        if (radius.localScale.x < targetRadius)
            radius.localScale += scaleSpeed * Time.deltaTime;

        if (cooldown <= 0)
        {
            if (isHealing)
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

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        AudioPlayer.Play(healSound, false, 1f, 1f, false, 0.6f);
        isHealing = true;
    }

    // On collision with enemy, apply damage
    public void OnTriggerExit2D(Collider2D collision)
    {
        // Get the enemy component
        AudioPlayer.Play(healSound, false, 0.8f, 0.8f, false, 0.6f);
        isHealing = false;
    }
}
