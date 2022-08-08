using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : Secondary
{
    // Knockback effect
    public float minKnockback = -350f;
    public float maxKnockback = -1000f;
    public float damage = 0;
    public float range = 15f;
    public bool forceKnockback = true;
    public ParticleSystem particle;

    // Virtual use method
    public override void Use()
    {
        if (cooldown <= 0 && !Dealer.isOpen)
        {
            cooldown = data.cooldown;
            ExplosiveHandler.CreateKnockback(ship.transform.position, range, 
                minKnockback, maxKnockback, damage, forceKnockback);
            AudioPlayer.Play(sound);
            particle.Play();
        }   
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
