using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Cards in deck
    private Dictionary<CardData, int> cards;

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
    private WeaponData activeWeapon;
    private float cooldown;

    // Scriptable and weapon reference
    private List<Weapon> weaponInstances;
    private StatData[] statSlots;
    private AbilityData[] powerupSlots;

    // On start, create decks
    public void Start()
    {
        // Set new card dictionary
        cards = new Dictionary<CardData, int>(); 

        // Create new dictionaries
        additions = new Dictionary<Stat, int>();
        multipliers = new Dictionary<Stat, float>();
        flags = new Dictionary<Stat, bool>();

        // Create starting slots
        weaponInstances = new List<Weapon>();
        statSlots = new StatData[_statAmount];
        powerupSlots = new AbilityData[_powerupAmount];

        // Set default slot
        if (useDefaultWeapon)
            SetupActive(defaultWeapon);

        // Set starting weapons
        if (useStartingCards)
        {
            for (int i = 0; i < startingWeaponCards.Count; i++)
                AddCard(startingWeaponCards[i]);
        }
    }

    // Calculate cooldown
    public void Update()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

        // Check if LMB input detected
        if (Input.GetKey(Keybinds.shoot)) UseActive();
        if (Input.GetKey(Keybinds.debug)) Dealer.active.OpenDealer();

        // Update primary cooldown
        if (cooldown > 0)
        cooldown -= Time.deltaTime;

        // Iterate through all weapon instances
        for (int i = 0; i < weaponInstances.Count; i++) 
        {
            if (weaponInstances[i] != null)
                weaponInstances[i].Use();
        }
    }

    // Set weapon card slot
    public void AddCard(CardData card)
    {
        // Check if card already in inventory
        if (cards.ContainsKey(card))
        {
            cards[card] += 1;
        }
        else
        {
            // Add new card
            cards.Add(card, 1);

            // Setup the card
            if (card is WeaponData)
                SetupPassive((WeaponData)card);
            else if (card is StatData)
                SetupStat((StatData)card);
            else if (card is AbilityData)
                SetupAbility((AbilityData)card);
        }
    }

    // Set primary card slot
    public void SetupActive(WeaponData weapon)
    {
        activeWeapon = weapon;
        cooldown = weapon.cooldown;
    }

    // Set passive card slot
    public void SetupPassive(WeaponData weapon)
    {
        // Create the new weapon instance
        Weapon newWeapon = Instantiate(weapon.obj, transform.position, Quaternion.identity).GetComponent<Weapon>();
        newWeapon.Setup(weapon, transform);
        weaponInstances.Add(newWeapon);

        // Check if player is parent
        if (weapon.setPlayerAsParent)
            newWeapon.transform.SetParent(transform);
    }

    // Set passive card slot
    public void SetupStat(StatData stat)
    {

    }

    // Set passive card slot
    public void SetupAbility(AbilityData ability)
    {

    }

    // Shoot method
    public void UseActive()
    {
        if (cooldown <= 0)
        {
            // Create bullet
            BulletHandler.active.CreateBullet(activeWeapon, barrel.position, rotator.rotation);
            cooldown = activeWeapon.cooldown;
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
