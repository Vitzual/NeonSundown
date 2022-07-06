using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Active instance
    public static Deck active;

    // Player instance
    public static Ship player;
    public static SecondaryData secondary;

    // Cards in deck
    private static Dictionary<CardData, int> cards;
    private static Dictionary<CardData, Weapon> upgradeables;
    private static Dictionary<CardData, Helper> helperUpgrades;

    // Multipliers 
    public static Dictionary<Stat, float> additions;
    public static Dictionary<Stat, float> multipliers;
    public static Dictionary<Stat, bool> flags;

    // Scriptable and weapon reference
    private List<Weapon> weaponInstances = new List<Weapon>();
    private List<Helper> helperInstances = new List<Helper>();

    // On awake, setup instance
    public void Awake()
    {
        // Set active instance
        active = this;
        secondary = null;
        
        // Set new card dictionary
        cards = new Dictionary<CardData, int>();
        upgradeables = new Dictionary<CardData, Weapon>();
        helperUpgrades = new Dictionary<CardData, Helper>();

        // Create new dictionaries
        additions = new Dictionary<Stat, float>();
        multipliers = new Dictionary<Stat, float>();
        flags = new Dictionary<Stat, bool>();

        // Create starting slots
        weaponInstances = new List<Weapon>();
        helperInstances = new List<Helper>();
    }
    
    // On start, create decks
    public void Start()
    {
        // Get player instance on the object
        player = GetComponent<Ship>();

        // Setup ship
        Events.active.SetupShip(Gamemode.shipData);
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

        // Iterate through all weapon instances
        for (int i = 0; i < helperInstances.Count; i++)
        {
            if (helperInstances[i] != null)
                helperInstances[i].CustomUpdate();
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
                upgradeables[card].Upgrade();
                if (cards[card] >= card.maximumAmount)
                    SynergyHandler.Add(card);
                return;
            }

            // If helper, apply to the instance
            if (helperUpgrades.ContainsKey(card))
            {
                helperUpgrades[card].Upgrade();
                if (cards[card] >= card.maximumAmount)
                    SynergyHandler.Add(card);
                return;
            }
        }
        else cards.Add(card, 1);

        // Check if card is maxed
        if (cards[card] >= card.maximumAmount)
            SynergyHandler.Add(card);

        // Setup the card
        if (card is WeaponData)
            SetupPassive((WeaponData)card);
        else if (card is StatData)
            SetupStat((StatData)card);
        else if (card is PrimaryData)
            SetupPrimary((PrimaryData)card);
        else if (card is SecondaryData)
            SetupSecondary((SecondaryData)card);
        else if (card is ChromaData)
            SetupChroma((ChromaData)card);
        else if (card is HelperData)
            SetupHelper((HelperData)card);
    }

    // Remove da card
    public void RemoveInstance(CardData card)
    {
        if (card is WeaponData)
        {
            WeaponData weapon = (WeaponData)card;
            foreach (Weapon data in weaponInstances)
            {
                if (data.GetWeapon() == weapon)
                {
                    Destroy(data.gameObject);
                    weaponInstances.Remove(data);
                    break;
                }
            }
        }
        else if (card is HelperData)
        {
            HelperData helperData = (HelperData)card;
            foreach (Helper helper in helperInstances)
            {
                if (helper.data == helperData)
                {
                    helper.Destroy();
                    helperInstances.Remove(helper);
                    break;
                }
            }
        }
    }

    // Takes a card
    public void TakeCard(CardData card)
    {
        // Check if card already in inventory
        if (cards.ContainsKey(card))
            cards.Remove(card);
    }

    // Returns a weapon card instance
    public Weapon GetWeaponInstance(WeaponData card)
    {
        if (upgradeables.ContainsKey(card))
            return upgradeables[card];
        else return null;
    }

    // Returns a helper card instance
    public Helper GetHelperInstance(HelperData card)
    {
        if (helperUpgrades.ContainsKey(card))
            return helperUpgrades[card];
        else return null;
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
        if (stat.value.multiply) AddMultiplier(stat.value.type, stat.value.modifier);
        else AddAddition(stat.value.type, stat.value.modifier);

        // Check player stats first
        player.UpdateStat(stat.value.type);

        // Update all weapon cards
        if (stat.applyToCards)
            foreach (KeyValuePair<CardData, Weapon> card in upgradeables)
                if (card.Value != null) card.Value.UpdateStat(stat.value.type);
    }

    // Set passive card slot
    public void SetupPrimary(PrimaryData primary)
    {
        // Add stat to the thing
        Debug.Log("Adding stat card " + primary.name + " to deck");

        // Go through and apply stats
        foreach(StatValue stat in primary.stats)
        {
            if (stat.multiply) AddMultiplier(stat.type, stat.modifier);
            else AddAddition(stat.type, stat.modifier);
            player.UpdateStat(stat.type);
        }
    }

    // Set passive card slot
    public void SetupSecondary(SecondaryData secondary)
    {
        // Check if secondary card already active
        if (Deck.secondary == secondary)
            GetSecondaryInstance().Upgrade();
        else
        {
            Debug.Log("Adding secondary card " + secondary.name + " to deck");
            player.SetSecondary(secondary);
            Deck.secondary = secondary;
        }
    }

    // Set passive card slot
    public void SetupHelper(HelperData helper)
    {
        Debug.Log("Adding helper card " + helper.name + " to deck");
        Helper newHelper = Instantiate(helper.obj, transform.position, Quaternion.identity).GetComponent<Helper>();
        newHelper.Setup(player, helper);
        helperInstances.Add(newHelper);
        helperUpgrades.Add(helper, newHelper);
    }

    // Get secondary instance
    public Secondary GetSecondaryInstance()
    {
        return player.GetSecondary();
    }

    // Set chroma card slot
    public void SetupChroma(ChromaData chroma)
    {
        Debug.Log("Adding chroma card " + chroma.name + " to deck");
        ChromaHandler.active.Setup(chroma);
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
}
