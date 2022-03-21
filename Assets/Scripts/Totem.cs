using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : Helper
{
    public AudioClip healSound;

    public float healAmount;
    public float healCooldown;

    public float movementSpeed;
    public float rotationSpeed;
    private float directionCooldown;

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

    // Move totem around randomly
    public void FixedUpdate()
    {
        // Check if dealer is open
        if (Dealer.isOpen) return;

        // Check position relative to player
        if (Vector2.Distance(transform.position, ship.transform.position) > 35f)
        {
            // Rotate towards the object
            if (rotationSpeed != 60f) rotationSpeed = 60f;
            float angle = Mathf.Atan2(ship.transform.position.y - transform.position.y,
                ship.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        else
        {
            // Rotate randomly
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            // Check direction cooldown
            if (directionCooldown <= 0f)
            {
                if (Random.Range(0f, 1f) > 0.5f) rotationSpeed = Random.Range(-80f, -40f);
                else rotationSpeed = Random.Range(40f, 80f);
                directionCooldown = 3f;
            }
            else directionCooldown -= Time.deltaTime;
        }

        // Move forward
        transform.position += transform.up * movementSpeed * Time.fixedDeltaTime;
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
