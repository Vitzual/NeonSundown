using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Active instance
    public static Deck active;

    // Player instance
    private static Ship player;

    // Cards in deck
    private static Dictionary<CardData, int> cards;
    private static Dictionary<CardData, Weapon> upgradeables;

    // Multipliers 
    public static Dictionary<Stat, float> additions;
    public static Dictionary<Stat, float> multipliers;
    public static Dictionary<Stat, bool> flags;

    // Scriptable and weapon reference
    private List<Weapon> weaponInstances = new List<Weapon>();

    // On start, create decks
    public void Start()
    {
        // Set active instance
        active = this;

        // Get player instance on the object
        player = GetComponent<Ship>();

        // Set new card dictionary
        cards = new Dictionary<CardData, int>();
        upgradeables = new Dictionary<CardData, Weapon>();

        // Create new dictionaries
        additions = new Dictionary<Stat, float>();
        multipliers = new Dictionary<Stat, float>();
        flags = new Dictionary<Stat, bool>();

        // Create starting slots
        weaponInstances = new List<Weapon>();
    }

    // Calculate cooldown
    public void Update()
    {
        // Check if something is open
        if (Dealer.isOpen) return;

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
            // If exists and not upgradeable, add
            cards[card] += 1;
            
            // If upgradeable, apply to the instance.
            if (upgradeables.ContainsKey(card))
            {
                WeaponData weaponData = (WeaponData)card;

                if (upgradeables[card].prestige &&
                    upgradeables[card].level != weaponData.prestigeLevels.Count)
                    upgradeables[card].Upgrade();
                else if (!upgradeables[card].prestige &&
                    upgradeables[card].level != weaponData.baseLevels.Count)
                    upgradeables[card].Upgrade();
                else if (!upgradeables[card].prestige &&
                    upgradeables[card].level == weaponData.baseLevels.Count)
                    upgradeables[card].Prestige();

                return;
            }
        }
        else cards.Add(card, 1);

        // Setup the card
        if (card is WeaponData)
            SetupPassive((WeaponData)card);
        else if (card is StatData)
            SetupStat((StatData)card);
        else if (card is PrimaryData)
            SetupPrimary((PrimaryData)card);
        else if (card is AbilityData)
            SetupAbility((AbilityData)card);
    }

    // Returns a weapon card instance
    public Weapon GetWeaponInstance(WeaponData card)
    {
        if (upgradeables.ContainsKey(card))
            return upgradeables[card];
        else return null;
    }

    // Get an amount of a card
    public int GetCardAmount(CardData card)
    {
        if (cards.ContainsKey(card))
            return cards[card];
        else return 0;
    }

    // Set passive card slot
    public void SetupPassive(WeaponData weapon)
    {
        // Create the new weapon instance
        Debug.Log("Adding passive weapon card " + weapon.name + " to deck");
        Weapon newWeapon = Instantiate(weapon.obj, transform.position, Quaternion.identity).GetComponent<Weapon>();
        newWeapon.Setup(weapon, transform);
        weaponInstances.Add(newWeapon);
        upgradeables.Add(weapon, newWeapon);

        // Check if player is parent
        if (weapon.setPlayerAsParent)
            newWeapon.transform.SetParent(transform);
    }

    // Set passive card slot
    public void SetupStat(StatData stat)
    {
        // Add stat to the thing
        Debug.Log("Adding stat card " + stat.name + " to deck");
        if (stat.multiply) AddMultiplier(stat.type, stat.modifier);
        else AddAddition(stat.type, stat.modifier);

        // Check player stats first
        if (stat.applyToPlayer)
            player.UpdateStat(stat.type, stat.modifier, stat.multiply);

        // Update all weapon cards
        if (stat.applyToCards)
            foreach (KeyValuePair<CardData, Weapon> card in upgradeables)
                if (card.Value != null) card.Value.UpdateStat(stat.type);
    }

    // Set passive card slot
    public void SetupPrimary(PrimaryData primary)
    {
        // Add stat to the thing
        Debug.Log("Adding stat card " + primary.name + " to deck");

        // Go through and apply stats
        foreach(PrimaryData.StatType stat in primary.stats)
        {
            if (stat.multiply) AddMultiplier(stat.type, stat.modifier);
            else AddAddition(stat.type, stat.modifier);
            player.UpdateStat(stat.type, stat.modifier, stat.multiply);
        }
    }

    // Set passive card slot
    public void SetupAbility(AbilityData ability)
    {
        Debug.Log("Adding ability card " + ability.name + " to deck");
    }

    // Setup starting cards
    public void SetupStartingCards()
    {
        // Add starting card if arena has one
        for (int a = 0; a < Gamemode.arena.startingCards.Count; a++)
            for (int b = 0; b < Gamemode.arena.startingCards[a].amount; b++)
                AddCard(Gamemode.arena.startingCards[a].card);
    }

    // Get deck cards
    public Dictionary<CardData, int> GetCards()
    {
        return cards;
    }

    // Calculate stat
    public static float CalculateStat(Stat type, float amount)
    {
        return (amount + GetAdditions(type)) * GetMultiplier(type);
    }

    // Get multiplier
    public static float GetAdditions(Stat type)
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
        return 1;
    }

    // Get flag
    public static bool GetFlag(Stat type)
    {
        if (flags.ContainsKey(type))
            return flags[type];
        else return false;
    }

    // Add a multiplier
    public static void AddAddition(Stat type, float amount)
    {
        if (additions.ContainsKey(type))
            additions[type] += amount;
        else additions.Add(type, amount);
    }

    // Add a multiplier
    public static void AddMultiplier(Stat type, float amount)
    {
        if (multipliers.ContainsKey(type))
            multipliers[type] += amount;
        else multipliers.Add(type, amount);
    }

    // Get a stat
    public static float GetStat(Stat type)
    {
        return player.GetStat(type);
    }
}
