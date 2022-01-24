using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Multipliers 
    public static Dictionary<Stat, int> additions;
    public static Dictionary<Stat, float> multipliers;
    public static Dictionary<Stat, bool> flags;

    // Default card
    public WeaponData defaultWeapon;
    public bool useDefaultCard;

    // Deck size
    public int _weaponAmount;
    public int _statAmount;
    public int _powerupAmount;

    // Deck slots
    public WeaponData[] weaponSlots;
    public float[] weaponCooldowns;
    public StatData[] statSlots;
    public PowerupData[] powerupSlots;

    // On start, create decks
    public void Start()
    {
        // Create starting slots
        weaponSlots = new WeaponData[_weaponAmount];
        statSlots = new StatData[_statAmount];
        powerupSlots = new PowerupData[_powerupAmount];

        // Set default slot
        if (useDefaultCard && weaponSlots.Length > 0)
            weaponSlots[0] = defaultWeapon;
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

    // Shoot method
    public void Shoot()
    {
        for(int i = 0; i < weaponSlots.Length; i++)
            if (weaponSlots[i] != null && weaponCooldowns[i] <= 0)
                weaponSlots[i].Shoot();
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
