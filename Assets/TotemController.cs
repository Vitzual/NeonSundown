using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemController : Secondary
{
    public Totem newTotem;
    public Totem oldTotem;
    public float healAmount;
    public float healCooldown;
    public float redeployCooldown;
    private AudioSource audioSource;

    // Get the audio source
    public override void Setup(Ship ship, SecondaryData data)
    {
        audioSource = GetComponent<AudioSource>();
        redeployCooldown = data.cooldown;
        base.Setup(ship, data);
    }

    // Teleport the ship to mouse cursor
    public override void Use()
    {
        if (cooldown <= 0 && !Dealer.isOpen)
        {
            // Reset cooldown
            cooldown = redeployCooldown;

            // Create particle at location
            if (oldTotem != null) Destroy(oldTotem);

            // Create totem at mouse location
            if (Controller.controller.activeSelf) oldTotem = Instantiate(newTotem, Controller.controller.transform.position, Quaternion.identity);
            else oldTotem = Instantiate(newTotem, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

            // Play sound
            audioSource.volume = Settings.sound;
            audioSource.Play();
        }
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
