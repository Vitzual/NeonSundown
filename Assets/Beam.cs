using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Laser
{
    // Particle system
    public new ParticleSystem particleSystem;
    public ParticleSystemRenderer particleRenderer;
    public AudioSource audioSource;

    // Update beam timer
    private float timer, endAtTime, endEarlyTime;

    public void SetupBeam(Transform parent, float width, float length, float spread,
        AudioClip sound, Material beamMaterial, bool useSound)
    {
        // Setup sound effect
        audioSource.clip = sound;
        if (useSound) audioSource.volume = Settings.sound;
        else audioSource.volume = 0f;
        audioSource.Play();

        // Set beam variables
        timer = 0f;
        endAtTime = sound.length;
        endEarlyTime = sound.length - 1.5f;

        // Setup beam effects
        particleRenderer.trailMaterial = beamMaterial;
        particleRenderer.material = beamMaterial;
        SetupLaser(parent, width, length, spread);
    }

    public override void Move()
    {
        timer += Time.deltaTime;
        if (timer >= endAtTime) Destroy();
        else if (timer >= endEarlyTime) base.Move();
    }

    public void EndEarly()
    {
        if (timer < endEarlyTime)
        {
            timer = endEarlyTime;
            audioSource.time = timer;
            particleSystem.Stop();
            Events.active.ResetCooldown();
        }
    }
}
