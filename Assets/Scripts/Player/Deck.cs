using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // List of all upgrades
    public class UpgradeInfo
    {
        public UpgradeInfo(UpgradeData upgrade, int quality)
        {
            this.upgrade = upgrade;
            this.quality = quality;
        }

        public UpgradeData upgrade;
        public int quality;
    }

    // Active instance
    public static Deck active;

    // Player instance
    public static Ship player;
    
    // Cards in deck
    private static Dictionary<CardData, int> cards;
    private static Dictionary<CardData, List<UpgradeInfo>> upgrades;

    // Multipliers 
    public static Dictionary<Stat, float> additions;
    public static Dictionary<Stat, float> multipliers;
    public static Dictionary<Stat, bool> flags;

    // On awake, setup instance
    public void Awake()
    {
        // Get player instance on the object
        player = GetComponent<Ship>();

        // Set active instance
        active = this;
        
        // Set new card dictionary
        cards = new Dictionary<CardData, int>();
        upgrades = new Dictionary<CardData, List<UpgradeInfo>>();

        // Create new dictionaries
        additions = new Dictionary<Stat, float>();
        multipliers = new Dictionary<Stat, float>();
        flags = new Dictionary<Stat, bool>();
    }
    
    // On start, create decks
    public void Start()
    {
        // Setup ship
        Events.active.SetupShip(Gamemode.shipData);
    }

    // Set weapon card slot
    public void AddCard(CardData card)
    {
        // Check if card already in inventory
        if (cards.ContainsKey(card)) cards[card] += 1;
        else cards.Add(card, 1);

        // Check if card is maxed
        if (cards[card] >= card.maximumAmount)
            SynergyHandler.Add(card);

        // Setup the card
        if (card.isShipOnlyCard)
        {
            // Add multipliers and additions
            if (card is StatData)
            {
                StatData statData = (StatData)card;
                foreach (StatValue stat in statData.stats)
                {
                    if (stat.multiply) player.AddMultiplier(stat.type, stat.modifier);
                    else player.AddAddition(stat.type, stat.modifier);
                }
            }

            // Invoke player update
            player.AddCard(card);
        }
        else
        {
            // Add multipliers and additions
            if (card is StatData)
            {
                StatData statData = (StatData)card;
                foreach (StatValue stat in statData.stats)
                {
                    if (stat.multiply) AddMultiplier(stat.type, stat.modifier);
                    else AddAddition(stat.type, stat.modifier);
                }
            }

            // Invoke update event
            Events.active.AddCard(card);
        }
    }

    // Upgrades a card
    public void UpgradeCard(CardData card, UpgradeData upgrade, int quality)
    {
        // Add upgrade to upgrades list
        if (!upgrades.ContainsKey(card)) upgrades.Add(card, new List<UpgradeInfo>());
        upgrades[card].Add(new UpgradeInfo(upgrade, quality));

        if (card is WeaponData)
        {
            Weapon weapon = player.GetWeapon((WeaponData)card);
            weapon.Upgrade(upgrade, quality);
        }
        else if (card is HelperData)
        {
            Helper helper = player.GetHelper((HelperData)card);
            helper.Upgrade(upgrade, quality);
        }
        else if (card is SecondaryData)
        {
            Secondary secondary = player.GetSecondary((SecondaryData)card);
            secondary.Upgrade(upgrade, quality);
        }
        else return;
    }
    
    // Set passive card slot
    public void SetupStat(StatData statData)
    {
        // Add stat to the thing
        Debug.Log("Adding stat card " + statData.name + " to deck");

        // Go through and apply stats
        foreach (StatValue stat in statData.stats)
        {
            if (stat.multiply) AddMultiplier(stat.type, stat.modifier);
            else AddAddition(stat.type, stat.modifier);
        }

        // Check player stats first
        Events.active.AddCard(statData);
    }

    // Takes a card
    public void TakeCard(CardData card)
    {
        // Check if card already in inventory
        if (cards.ContainsKey(card))
            cards.Remove(card);
    }

    // Get an amount of a card
    public int GetCardAmount(CardData card)
    {
        if (cards.ContainsKey(card))
            return cards[card];
        else return -1;
    }

    // Get an amount of a card
    public bool HasCard(CardData card)
    {
        if (cards != null)
            return cards.ContainsKey(card);
        else return false;
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
            multipliers[type] *= amount;
        else multipliers.Add(type, amount);
    }

    // Get a stat
    public static float GetStat(Stat type) { return player.GetStat(type); }
    public static float GetDefaultStat(Stat type) { return player.GetDefaultStat(type); }

    // Get upgades
    public static List<UpgradeInfo> GetUpgrades(CardData card)
    {
        if (upgrades.ContainsKey(card))
            return upgrades[card];
        else return null;
    }
}
