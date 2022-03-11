using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : Secondary
{
    // Knockback effect
    public float minKnockback = -350f;
    public float maxKnockback = -1000f;
    public float range = 15f;
    public ParticleSystem particle;

    // Virtual use method
    public override void Use()
    {
        if (cooldown <= 0)
        {
            cooldown = data.cooldown;
            ExplosiveHandler.CreateKnockback(ship.transform.position, range, minKnockback, maxKnockback);
            AudioPlayer.Play(sound);
            particle.Play();
        }
    }

    // Upgrade pulse
    public override void Upgrade()
    {
        // Apply new effects
        if (level < data.levels.Count)
        {
            StatValue stat = data.levels[level].stat;
            switch (stat.type)
            {
                case Stat.Range:
                    if (stat.multiply) range *= stat.modifier;
                    else range += stat.modifier;
                    break;

                case Stat.Knockback:
                    if (stat.multiply)
                    {
                        maxKnockback *= stat.modifier;
                        minKnockback *= stat.modifier;
                    }
                    else
                    {
                        maxKnockback += stat.modifier;
                        minKnockback += stat.modifier;
                    }
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
            case Stat.Range:
                return range;
            case Stat.Knockback:
                return maxKnockback;
            default:
                return base.GetStat(stat);
        }
    }
}
