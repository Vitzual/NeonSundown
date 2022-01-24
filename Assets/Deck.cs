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
    public WeaponData defaultWeapon;
    public bool useDefaultCard;

    // Deck size
    public int _weaponAmount;
    public int _statAmount;
    public int _powerupAmount;

    // Deck slots
    private WeaponData[] weaponSlots;
    private float[] weaponCooldowns;
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
        weaponSlots = new WeaponData[_weaponAmount];
        weaponCooldowns = new float[_weaponAmount];
        statSlots = new StatData[_statAmount];
        powerupSlots = new PowerupData[_powerupAmount];

        // Set default slot
        if (useDefaultCard && weaponSlots.Length > 0)
            SetWeaponSlot(0, defaultWeapon);
    }

    // Calculate cooldown
    public void Update()
    {
        if (Input.GetKey(Keybinds.shoot)) Shoot();

        for(int i = 0; i < weaponCooldowns.Length; i++)
        {
            if (weaponCooldowns[i] > 0)
                weaponCooldowns[i] -= Time.deltaTime;
        }
    }

    // Set card slot
    public void SetWeaponSlot(int index, WeaponData weapon)
    {
        weaponSlots[index] = weapon;
        weaponCooldowns[index] = weapon.cooldown;
    }

    // Shoot method
    public void Shoot()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null && weaponCooldowns[i] <= 0)
            {
                BulletHandler.active.CreateBullet(weaponSlots[i], barrel.position, transform.rotation);
                weaponCooldowns[i] = weaponSlots[i].cooldown;
            }
        }
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
