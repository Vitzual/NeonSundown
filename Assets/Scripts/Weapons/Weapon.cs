using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Multipliers 
    protected Dictionary<Stat, int> additions = new Dictionary<Stat, int>();
    protected Dictionary<Stat, float> multipliers = new Dictionary<Stat, float>();

    // Prestige and base models
    [SerializeField]
    protected GameObject baseModel;
    [SerializeField]
    protected GameObject prestigeModel;

    // Weapon variables
    protected float damage;
    protected float cooldown;
    protected float moveSpeed;
    protected float bloom;
    protected float pierces;
    protected float bullets;
    protected float lifetime;

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
        SetupStats();
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

        // Update stats
        UpdateStat(newLevel.stat);

        // Increase level
        level += 1;
    }

    // Prestiges a weapon
    public virtual void Prestige()
    {
        // Reset stats and level
        SetupStats();
        level = 0;
        prestige = true;

        // Reset additions and multipliers on the card
        additions = new Dictionary<Stat, int>();
        multipliers = new Dictionary<Stat, float>();

        // Set prestige model to true
        if (prestigeModel != null)
        {
            baseModel.SetActive(false);
            prestigeModel.SetActive(true);
        }
    }

    // Calculate stat
    public void UpdateStat(Stat type)
    {
        switch (type)
        {
            case Stat.Damage:
                damage = Deck.CalculateStat(type, weapon.damage) 
                    + GetAdditions(type) * GetMultiplier(type);
                break;
            case Stat.Cooldown:
                cooldown = Deck.CalculateStat(type, weapon.cooldown)
                    + GetAdditions(type) * GetMultiplier(type);
                break;
            case Stat.Speed:
                moveSpeed = Deck.CalculateStat(type, weapon.moveSpeed)
                    + GetAdditions(type) * GetMultiplier(type);
                break;
            case Stat.Bloom:
                bloom = Deck.CalculateStat(type, weapon.bloom)
                    + GetAdditions(type) * GetMultiplier(type);
                break;
            case Stat.Pierces:
                pierces = Deck.CalculateStat(type, weapon.pierces)
                    + GetAdditions(type) * GetMultiplier(type);
                break;
            case Stat.Bullets:
                bullets = Deck.CalculateStat(type, weapon.bullets)
                    + GetAdditions(type) * GetMultiplier(type);
                break;
            case Stat.Lifetime:
                lifetime = Deck.CalculateStat(type, weapon.lifetime)
                    + GetAdditions(type) * GetMultiplier(type);
                break;
        }
    }

    // Setup stats
    protected void SetupStats()
    {
        damage = weapon.damage;
        cooldown = weapon.cooldown;
        moveSpeed = weapon.moveSpeed;
        bloom = weapon.bloom;
        pierces = weapon.pierces;
        bullets = weapon.bullets;
        lifetime = weapon.lifetime;
    }

    // Get multiplier
    protected int GetAdditions(Stat type)
    {
        if (additions.ContainsKey(type))
            return additions[type];
        else return 0;
    }

    // Get multiplier
    protected float GetMultiplier(Stat type)
    {
        if (multipliers.ContainsKey(type))
            return multipliers[type];
        else return 1;
    }

    // Add a multiplier
    protected void AddAddition(Stat type, int amount)
    {
        if (additions.ContainsKey(type))
            additions[type] += amount;
        else additions.Add(type, amount);
    }

    // Add a multiplier
    protected void AddMultiplier(Stat type, float amount)
    {
        if (multipliers.ContainsKey(type))
            multipliers[type] *= amount;
        else multipliers.Add(type, amount);
    }
}
