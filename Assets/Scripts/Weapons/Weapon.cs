using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Multipliers 
    public Dictionary<Stat, int> additions;
    public Dictionary<Stat, float> multipliers;

    // Weapon level
    public int level = 0;
    public bool prestige = false;

    // Target transform
    protected Transform target;

    // Weapon data
    protected WeaponData weapon;

    // Set the weapon data
    public virtual void Setup(WeaponData data, Transform target = null)
    {
        this.target = target;
        weapon = data;
    }
    
    // Base weapon methods
    public virtual void Use() 
    { 
        // Override in parent class to tell this weapon what to do
    }

    // Upgrades a weapon
    public virtual void Upgrade() 
    {
        // Get the current level
        WeaponData.Level newLevel;
        if (prestige) newLevel = weapon.prestigeLevels[level];
        else newLevel = weapon.baseLevels[level];

        // Add the upgrade
        if (newLevel.multiply)
            AddMultiplier(newLevel.stat, newLevel.modifier);
        else AddAddition(newLevel.stat, (int)newLevel.modifier);

        // Increase level
        level += 1;
    }

    // Prestiges a weapon
    public virtual void Prestige()
    {
        additions = new Dictionary<Stat, int>();
        multipliers = new Dictionary<Stat, float>();
        level = 0;
    }

    // Calculate stat
    public float CalculateStat(Stat type)
    {
        return Deck.CalculateStat(Stat.Damage, weapon.damage) + GetAdditions(type) * GetMultiplier(type);
    }

    // Get multiplier
    public int GetAdditions(Stat type)
    {
        if (additions.ContainsKey(type))
            return additions[type];
        else return 0;
    }

    // Get multiplier
    public float GetMultiplier(Stat type)
    {
        if (multipliers.ContainsKey(type))
            return multipliers[type];
        else return 1;
    }

    // Add a multiplier
    public void AddAddition(Stat type, int amount)
    {
        if (additions.ContainsKey(type))
            additions[type] += amount;
        else additions.Add(type, amount);
    }

    // Add a multiplier
    public void AddMultiplier(Stat type, float amount)
    {
        if (multipliers.ContainsKey(type))
            multipliers[type] *= amount;
        else multipliers.Add(type, amount);
    }
}
