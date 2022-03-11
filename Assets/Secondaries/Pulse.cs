using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : Secondary
{
    // Knockback effect
    public float minKnockback = -350f;
    public float maxKnockback = -1000f;
    public float size = 15f;
    public ParticleSystem particle;

    // Virtual use method
    public override void Use()
    {
        if (cooldown <= 0)
        {
            cooldown = data.cooldown;
            ExplosiveHandler.CreateKnockback(ship.transform.position, size, minKnockback, maxKnockback);
            AudioPlayer.Play(sound);
            particle.Play();
        }
    }
}
