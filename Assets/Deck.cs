using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Multipliers 
    public static Dictionary<Stat, int> additions;
    public static Dictionary<Stat, float> multipliers;
    public static Dictionary<Stat, bool> flags;

    // Barrel location
    public Transform barrel;

    // Default card
    public PrimaryData defaultWeapon;
    public bool useDefaultCard;

    // Deck size
    public int _statAmount;
    public int _powerupAmount;

    // Deck slots
    private PrimaryData primary;
    private PrimaryData secondary;
    private float primaryCooldown;
    private float secondaryCooldown;

    private StatData[] statSlots;
    private PowerupData[] powerupSlots;

    // On start, create decks
    public void Start()
    {
        // Create new dictionaries
        additions = new Dictionary<Stat, int>();
        multipliers = new Dictionary<Stat, float>();
        flags = new Dictionary<Stat, bool>();

        // Create starting slots
        statSlots = new StatData[_statAmount];
        powerupSlots = new PowerupData[_powerupAmount];

        // Set default slot
        if (useDefaultCard)
            SetWeaponSlot(true, defaultWeapon);
    }

    // Calculate cooldown
    public void Update()
    {
        if (Input.GetKey(Keybinds.shoot)) Shoot(true);

        if (primaryCooldown > 0)
            primaryCooldown -= Time.deltaTime;

        if (secondaryCooldown > 0)
            secondaryCooldown -= Time.deltaTime;
    }

    // Set card slot
    public void SetWeaponSlot(bool isPrimary, PrimaryData weapon)
    {
        if (isPrimary)
        {
            primary = weapon;
            primaryCooldown = weapon.cooldown;
        }
        else
        {
            secondary = weapon;
            secondaryCooldown = weapon.cooldown;
        }
    }

    // Shoot method
    public void Shoot(bool isPrimary)
    {
        if (isPrimary)
        {
            if (primaryCooldown <= 0)
            {
                BulletHandler.active.CreateBullet(primary, barrel.position, transform.rotation);
                primaryCooldown = primary.cooldown;
            }
        }
        else
        {
            if (secondaryCooldown <= 0)
            {
                BulletHandler.active.CreateBullet(secondary, barrel.position, transform.rotation);
                secondaryCooldown = secondary.cooldown;
            }
        }
    }

    // Calculate stat
    public static float CalculateStat(Stat type, float amount)
    {
        return amount + GetAdditions(type) * GetMultiplier(type);
    }

    // Get multiplier
    public static int GetAdditions(Stat type)
    {
        if (additions.ContainsKey(type))
            return additions[type];
        else return 0;
    }

    // Get multiplier
    public static float GetMultiplier(Stat type)
    {
        if (multipliers.ContainsKey(type))
            return multipliers[type];
        else return 1;
    }

    // Get flag
    public static bool GetFlag(Stat type)
    {
        if (flags.ContainsKey(type))
            return flags[type];
        else return false;
    }
}
