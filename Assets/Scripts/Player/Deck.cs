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
    public Transform rotator;

    // Default card
    public WeaponData defaultWeapon;
    public bool useDefaultWeapon;

    // Starting cards
    public List<WeaponData> startingWeaponCards;
    public List<StatData> startingStatCards;
    public List<AbilityData> startingAbilityCards;
    public bool useStartingCards;

    // Deck size
    public int _weaponAmount;
    public int _statAmount;
    public int _powerupAmount;

    // Deck slots
    private WeaponData primary;
    private float cooldown;

    // Scriptable and weapon reference
    private Weapon[] weaponInstances;
    private WeaponData[] weaponSlots;

    // Scriptable only
    private StatData[] statSlots;
    private AbilityData[] powerupSlots;

    // On start, create decks
    public void Start()
    {
        // Create new dictionaries
        additions = new Dictionary<Stat, int>();
        multipliers = new Dictionary<Stat, float>();
        flags = new Dictionary<Stat, bool>();

        // Create starting slots
        weaponInstances = new Weapon[_weaponAmount];
        weaponSlots = new WeaponData[_weaponAmount];
        statSlots = new StatData[_statAmount];
        powerupSlots = new AbilityData[_powerupAmount];

        // Set default slot
        if (useDefaultWeapon)
            SetPrimarySlot(defaultWeapon);

        // Set starting weapons
        if (useStartingCards)
        {
            for (int i = 0; i < startingWeaponCards.Count; i++)
                SetWeaponSlot(i, startingWeaponCards[i]);
        }
    }

    // Calculate cooldown
    public void Update()
    {
        // Check if LMB input detected
        if (Input.GetKey(Keybinds.shoot)) Shoot();

        // Update primary cooldown
        if (cooldown > 0)
            cooldown -= Time.deltaTime;

        // Iterate through all weapon instances
        for (int i = 0; i < weaponInstances.Length; i++) 
        {
            if (weaponInstances[i] != null)
                weaponInstances[i].Use();
        }
    }

    // Set weapon card slot
    public void SetWeaponSlot(int slot, WeaponData weapon)
    {
        if (slot < weaponSlots.Length)
        {
            // Remove old weapon
            if (weaponInstances[slot] != null)
                Destroy(weaponInstances[slot].gameObject);

            // Set new weapon SO
            weaponSlots[slot] = weapon;

            // Create the new weapon instance
            Weapon newWeapon = Instantiate(weapon.obj, transform.position, Quaternion.identity).GetComponent<Weapon>();
            newWeapon.Setup(weapon, transform);
            weaponInstances[slot] = newWeapon;

            // Check if player is parent
            if (weapon.setPlayerAsParent)
                newWeapon.transform.SetParent(transform);
        }
    }

    // Set primary card slot
    public void SetPrimarySlot(WeaponData weapon)
    {
        primary = weapon;
        cooldown = weapon.cooldown;
    }

    // Shoot method
    public void Shoot()
    {
        if (cooldown <= 0)
        {
            BulletHandler.active.CreateBullet(primary, barrel.position, rotator.rotation);
            cooldown = primary.cooldown;
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
