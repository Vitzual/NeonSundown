using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Active instance
    public static Deck active;

    // Cards in deck
    private Dictionary<CardData, int> cards;
    private Dictionary<CardData, Weapon> upgradeables;

    // Multipliers 
    public static Dictionary<Stat, int> additions;
    public static Dictionary<Stat, float> multipliers;
    public static Dictionary<Stat, bool> flags;

    // Scriptable and weapon reference
    private List<Weapon> weaponInstances = new List<Weapon>();

    // On start, create decks
    public void Start()
    {
        // Set active instance
        active = this;

        // Set new card dictionary
        cards = new Dictionary<CardData, int>();
        upgradeables = new Dictionary<CardData, Weapon>();

        // Create new dictionaries
        additions = new Dictionary<Stat, int>();
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
            if (!upgradeables.ContainsKey(card))
                cards[card] += 1;
            
            // If upgradeable, apply to the instance.
            else
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
        // Update all weapon cards
        foreach(KeyValuePair<CardData, Weapon> card in upgradeables)
            if (card.Value != null) card.Value.UpdateStat(stat.type);
    }

    // Set passive card slot
    public void SetupAbility(AbilityData ability)
    {

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
