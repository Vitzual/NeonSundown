using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Multipliers 
    protected Dictionary<Stat, float> additions = new Dictionary<Stat, float>();
    protected Dictionary<Stat, float> multipliers = new Dictionary<Stat, float>();
    
    // Weapon variables
    [HideInInspector]
    public float damage, damageMultiplier = 1f, cooldown, moveSpeed, bloom, pierces, bullets, lifetime,
        range, knockback, splitshots, size, stunLength, critical, rotateSpeed, speedDamageMultiplier,
        reverseShots = 0, richochets = 0;
    [HideInInspector]
    public bool explosiveRounds, autoLockRounds, speedAffectsDamage = false, informOnHit = false;

    // Weapon level
    [HideInInspector]
    public int level = 0;

    // Target transform
    protected Transform target;

    // Weapon data
    protected WeaponData weapon;
    
    // Set the weapon data
    public virtual void Setup(WeaponData data, Transform target = null)
    {
        Events.active.onAddCard += AddCard;
        this.target = target;
        weapon = data;
        damageMultiplier = 1f;
        informOnHit = data.informOnHit;
        SetupStats();
    }
    
    // Base weapon methods
    public virtual void Use() 
    { 
        // Override in parent class to tell this weapon what to do
    }

    // Upgrades a weapon
    public virtual void Upgrade(UpgradeData upgrade, int quality) 
    {
        if (upgrade.qualities[quality].special == UpgradeData.Special.None)
        {
            // Add positive effect
            Debug.Log(upgrade.positiveStat.ToString() + " Multiplier: " + GetMultiplier(upgrade.positiveStat));
            if (upgrade.positiveMultiplier) AddMultiplier(upgrade.positiveStat, upgrade.qualities[quality].positiveEffect);
            else AddAddition(upgrade.positiveStat, upgrade.qualities[quality].positiveEffect);
            Debug.Log(upgrade.positiveStat.ToString() + " New Multiplier: " + GetMultiplier(upgrade.positiveStat));

            // Update stats
            UpdateStat(upgrade.positiveStat);
        }
        else
        {
            if (upgrade.qualities[quality].special == UpgradeData.Special.Sense) reverseShots += 1;
        }

        // Add negative effect
        if (upgrade.negativeMultiplier) AddMultiplier(upgrade.negativeStat, upgrade.qualities[quality].negativeEffect);
        else AddAddition(upgrade.negativeStat, upgrade.qualities[quality].negativeEffect);

        // Update negative effect
        UpdateStat(upgrade.negativeStat);
    }

    // On add card event
    public virtual void AddCard(CardData card)
    {
        if (card is StatData)
        {
            StatData statData = (StatData)card;
            foreach (StatValue stat in statData.stats)
                UpdateStat(stat.type);
        }
    }

    public virtual void Destroy()
    {
        Events.active.onAddCard -= AddCard;
        Destroy(gameObject);
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
            case Stat.StunLength:
                stunLength = (Deck.CalculateStat(type, weapon.stun)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.BulletSize:
                size = (Deck.CalculateStat(type, weapon.bulletSize)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Range:
                range = (Deck.CalculateStat(type, weapon.range)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Criticals:
                critical = (Deck.CalculateStat(type, 0) + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Richochets:
                richochets = (Deck.CalculateStat(type, 0) + GetAdditions(type)) * GetMultiplier(type);
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
        UpdateStat(Stat.StunLength);
        UpdateStat(Stat.BulletSize);
        UpdateStat(Stat.Bullets);
        UpdateStat(Stat.Explosive);
        UpdateStat(Stat.Richochets);
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

            // Get splitshots
            case Stat.StunLength:
                return stunLength;

            // Get crit
            case Stat.Criticals:
                return critical;

            // Get bullet size
            case Stat.BulletSize:
                return size;

            // Get richochets
            case Stat.Richochets:
                return richochets;

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

            // Increase regen rate
            case Stat.StunLength:
                return weapon.stun;

            // Get crit
            case Stat.Criticals:
                return 0;

            // Get bullet size
            case Stat.BulletSize:
                return weapon.bulletSize;

            // Get richochets
            case Stat.Richochets:
                return 0;

            // Default case
            default:
                return 0;
        }
    }

    // Get multiplier
    public float GetAdditions(Stat type)
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
    public void AddAddition(Stat type, float amount)
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

    // Get weapon
    public WeaponData GetWeapon() { return weapon; }

    // Called if bullet is instructed to inform parent of hits
    public virtual void TargetHit(Entity entity) { }
}
