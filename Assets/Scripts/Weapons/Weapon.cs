using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Multipliers 
    protected Dictionary<Stat, float> additions = new Dictionary<Stat, float>();
    protected Dictionary<Stat, float> multipliers = new Dictionary<Stat, float>();

    // Prestige and base models
    [SerializeField]
    protected GameObject baseModel;
    [SerializeField]
    protected GameObject prestigeModel;

    // Weapon variables
    [HideInInspector]
    public float damage, cooldown, moveSpeed, bloom, pierces, 
        bullets, lifetime, range, knockback, splitshots;

    // Weapon level
    [HideInInspector]
    public int level = 0;
    [HideInInspector]
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
        else AddAddition(newLevel.stat, newLevel.modifier);

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
        additions = new Dictionary<Stat, float>();
        multipliers = new Dictionary<Stat, float>();

        // Set prestige model to true
        if (prestigeModel != null)
        {
            baseModel.SetActive(false);
            prestigeModel.SetActive(true);
        }
    }

    // Calculate stat
    public virtual void UpdateStat(Stat type)
    {
        switch (type)
        {
            case Stat.Damage:
                damage = (Deck.CalculateStat(type, weapon.damage) 
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Cooldown:
                cooldown = (Deck.CalculateStat(type, weapon.cooldown)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.MoveSpeed:
                moveSpeed = (Deck.CalculateStat(type, weapon.moveSpeed)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Spread:
                bloom = (Deck.CalculateStat(type, weapon.bloom)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Pierces:
                pierces = (Deck.CalculateStat(type, weapon.pierces)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Bullets:
                bullets = (Deck.CalculateStat(type, weapon.bullets)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Lifetime:
                lifetime = (Deck.CalculateStat(type, weapon.lifetime)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Knockback:
                knockback = (Deck.CalculateStat(type, weapon.knockback) 
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
        }
    }

    // Setup stats
    protected void SetupStats()
    {
        UpdateStat(Stat.Damage);
        UpdateStat(Stat.Cooldown);
        UpdateStat(Stat.MoveSpeed);
        UpdateStat(Stat.Spread);
        UpdateStat(Stat.Pierces);
        UpdateStat(Stat.Bullets);
        UpdateStat(Stat.Lifetime);
        UpdateStat(Stat.Range);
        UpdateStat(Stat.Knockback);
    }

    // Returns a stat
    public virtual float GetStat(Stat stat)
    {
        switch (stat)
        {
            // Upgrades the damage 
            case Stat.Damage:
                return damage;

            // Increases firerate 
            case Stat.Cooldown:
                return cooldown;

            // Increases bullets
            case Stat.Bullets:
                return bullets;

            // Increases piercing rounds
            case Stat.Pierces:
                return pierces;

            // Increases bullet lifetime
            case Stat.Lifetime:
                return lifetime;

            // Increases accuracy
            case Stat.Spread:
                return bloom;

            // Increase regen rate
            case Stat.Knockback:
                return knockback;

            // Get splitshots
            case Stat.Splitshot:
                return splitshots;

            // Default case
            default:
                return 0;
        }
    }

    // Returns a stat
    public virtual float GetDefaultStat(Stat stat)
    {
        switch (stat)
        {
            // Upgrades the damage 
            case Stat.Damage:
                return weapon.damage;

            // Increases firerate 
            case Stat.Cooldown:
                return weapon.cooldown;

            // Increases bullets
            case Stat.Bullets:
                return weapon.bullets;

            // Increases piercing rounds
            case Stat.Pierces:
                return weapon.pierces;

            // Increases bullet lifetime
            case Stat.Lifetime:
                return weapon.lifetime ;

            // Increases accuracy
            case Stat.Spread:
                return weapon.bloom;

            // Increase regen rate
            case Stat.Knockback:
                return weapon.knockback;

            // Default case
            default:
                return 0;
        }
    }

    // Get multiplier
    protected float GetAdditions(Stat type)
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
    protected void AddAddition(Stat type, float amount)
    {
        if (additions.ContainsKey(type))
            additions[type] += amount;
        else additions.Add(type, amount);
    }

    // Add a multiplier
    protected void AddMultiplier(Stat type, float amount)
    {
        Debug.Log(type + " added " + amount);
        if (multipliers.ContainsKey(type))
            multipliers[type] += amount;
        else multipliers.Add(type, amount);
    }
}
