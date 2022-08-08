using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : Secondary
{
    // Get the teleport sound
    public ParticleSystem particle;
    private float customCooldown;
    private AudioSource audioSource;

    // Get the audio source
    public override void Setup(Ship ship, SecondaryData data)
    {
        audioSource = GetComponent<AudioSource>();
        customCooldown = data.cooldown;
        base.Setup(ship, data);
    }

    // Teleport the ship to mouse cursor
    public override void Use()
    {
        if (cooldown <= 0 && !Dealer.isOpen)
        {
            // Reset cooldown
            cooldown = customCooldown;

            // Create particle at location
            Instantiate(particle, transform.position, Quaternion.identity);

            // Move ship to cursor
            if (Controller.controller.activeSelf)
            {
                ship.transform.position = Controller.controller.transform.position;
            }
            else
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ship.transform.position = mousePos;
            }

            // Create particle at new location
            Instantiate(particle, transform.position, Quaternion.identity);

            // Play sound
            audioSource.volume = Settings.sound;
            audioSource.Play();
        }
    }

    // Overrides the get stat function
    public override float GetStat(Stat stat)
    {
        if (stat == Stat.Cooldown)
            return customCooldown;
        else return -1;
    }
}
