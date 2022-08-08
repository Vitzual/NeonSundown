using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : Entity
{
    // List of all upgrades
    public class UpgradeInfo
    {
        public UpgradeInfo(UpgradeData upgrade, int quality)
        {
            this.upgrade = upgrade;
            this.quality = quality;
        }

        private UpgradeData upgrade;
        private int quality;
    }
    private List<UpgradeInfo> upgrades = new List<UpgradeInfo>();

    // Multipliers 
    protected Dictionary<Stat, float> additions = new Dictionary<Stat, float>();
    protected Dictionary<Stat, float> multipliers = new Dictionary<Stat, float>();

    // Weapon variables
    [HideInInspector]
    public float moveSpeed;

    // Ship instance
    protected Ship ship;
    [HideInInspector] public HelperData data;
    [HideInInspector] public int level;

    // Virtual setup method
    public virtual void Setup(Ship ship, HelperData data)
    {
        this.ship = ship;
        this.data = data;

        UpdateStat(Stat.StunLength);
    }

    // Custom update
    public virtual void CustomUpdate() { }

    // Upgrades the card
    public virtual void Upgrade(UpgradeData upgrade, int quality)
    {
        // Add upgrade to upgrades class
        upgrades.Add(new UpgradeInfo(upgrade, quality));

        if (upgrade.qualities[quality].special == UpgradeData.Special.None)
        {
            // Add positive effect
            if (upgrade.positiveMultiplier) AddMultiplier(upgrade.positiveStat, upgrade.qualities[quality].positiveEffect);
            else AddAddition(upgrade.positiveStat, upgrade.qualities[quality].positiveEffect);

            // Add negative effect
            if (upgrade.negativeMultiplier) AddMultiplier(upgrade.negativeStat, upgrade.qualities[quality].negativeEffect);
            else AddAddition(upgrade.negativeStat, upgrade.qualities[quality].negativeEffect);
        }
        else
        {
            // Do custom stuff here
        }
    }

    // Returns a formatted string on the upgrade info
    public virtual string GetUpgradeString()
    {
        if (level < data.levels.Count)
            return data.levels[level].description;
        else return "LEVEL MAX";
    }

    public virtual void SetTarget(Entity entity)
    {
        // Do nothing on base
    }

    // Calculate stat
    public virtual void UpdateStat(Stat type)
    {
        switch (type)
        {
            case Stat.MoveSpeed:
                moveSpeed = (Deck.CalculateStat(type, data.moveSpeed)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
        }
    }

    // Returns a stat
    public virtual float GetStat(Stat stat)
    {
        switch (stat)
        {
            // Get splitshots
            case Stat.StunLength:
                return stunLength;

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
            // Get crit
            case Stat.Criticals:
                return 0;

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
        if (multipliers.ContainsKey(type))
            multipliers[type] += amount;
        else multipliers.Add(type, amount);
    }

    public List<UpgradeInfo> GetUpgrades() { return upgrades; }
}
