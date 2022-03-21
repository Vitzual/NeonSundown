using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : Helper
{
    public AudioClip healSound;

    public float healAmount;
    public float healCooldown;
    public float redeployCooldown;

    public float movementSpeed;
    public float rotationSpeed;
    private float directionCooldown;

    public Transform radius;
    public float targetRadius;
    public Vector3 scaleSpeed;
    public float cooldown;
    public bool isHealing;

    // On start, play animation
    public void Update()
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

    // Move totem around randomly
    public void FixedUpdate()
    {
        // Check if rotating (lock target)
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        transform.position += transform.up * movementSpeed * Time.fixedDeltaTime;

        // Check direction cooldown
        if (directionCooldown <= 0f)
        {
            if (Random.Range(0f, 1f) > 0.5f) rotationSpeed = Random.Range(-80f, -40f);
            else rotationSpeed = Random.Range(40f, 80f);
            directionCooldown = 3f;
        }
        else directionCooldown -= Time.deltaTime;
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
