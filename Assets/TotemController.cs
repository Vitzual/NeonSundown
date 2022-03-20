using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemController : Secondary
{
    public Totem newTotem;
    private Totem oldTotem;
    public float healAmount;
    public float healCooldown;
    public float redeployCooldown;

    public float movementSpeed;
    public float rotationSpeed;
    private float directionCooldown;

    // Get the audio source
    public override void Setup(Ship ship, SecondaryData data)
    {
        redeployCooldown = data.cooldown;
        base.Setup(ship, data);
    }

    // Move totem around randomly
    public void FixedUpdate()
    {
        // Check if old totem null
        if (oldTotem == null || Dealer.isOpen) return;

        // Check if rotating (lock target)
        oldTotem.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        oldTotem.transform.position += oldTotem.transform.up * movementSpeed * Time.fixedDeltaTime;

        // Check direction cooldown
        if (directionCooldown <= 0f)
        {
            if (Random.Range(0f, 1f) > 0.5f) rotationSpeed = Random.Range(-80f, -40f);
            else rotationSpeed = Random.Range(40f, 80f);
            directionCooldown = 3f;
        }
        else directionCooldown -= Time.deltaTime;
    }

    // Teleport the ship to mouse cursor
    public override void Use()
    {
        if (cooldown <= 0 && !Dealer.isOpen)
        {
            // Reset cooldown
            cooldown = redeployCooldown;

            // Create particle at location
            if (oldTotem != null) Destroy(oldTotem.gameObject);

            // Create totem at mouse location
            if (Controller.controller.activeSelf) oldTotem = Instantiate(newTotem, Controller.controller.transform.position, Quaternion.identity);
            else oldTotem = Instantiate(newTotem, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            oldTotem.totemController = this;
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

    // Override destroy
    public override void Destroy()
    {
        if (oldTotem != null) Destroy(oldTotem.gameObject);
        base.Destroy();
    }
}
