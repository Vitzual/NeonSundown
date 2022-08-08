using System.Collections.Generic;
using UnityEngine;

public class Secondary : MonoBehaviour
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

    // Ship instance
    public bool useCustomUpdate = false;
    public AudioClip sound;
    protected Ship ship;
    [HideInInspector]
    public SecondaryData data;
    protected float cooldown = 0;
    [HideInInspector]
    public int level;

    // Virtual setup method
    public virtual void Setup(Ship ship, SecondaryData data)
    {
        this.ship = ship;
        this.data = data;
    }

    // Virtual update method
    public void Update()
    {
        // Update secondary instance
        if (!Dealer.isOpen && cooldown > 0)
            cooldown -= Time.deltaTime;
        if (Input.GetKey(Keybinds.secondary) || Input.GetAxis("Secondary") > 0.5) Use();

        // Check if secondary has custom update
        if (useCustomUpdate) CustomUpdate();
    }
    
    // Custom update method
    public virtual void CustomUpdate()
    {

    }

    // Virtual use method
    public virtual void Use()
    {
        if (cooldown <= 0)
            cooldown = ship.cooldown;
    }

    // Destroys the instance
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

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

    // Calculate stat
    public virtual void UpdateStat(Stat type)
    {
        switch (type)
        {
            case Stat.Cooldown:
                cooldown = (Deck.CalculateStat(type, data.cooldown)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
        }
    }

    // Returns a stat
    public virtual float GetStat(Stat stat)
    {
        switch (stat)
        {
            // Increases firerate 
            case Stat.Cooldown:
                return cooldown;

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
            // Increases firerate 
            case Stat.Cooldown:
                return data.cooldown;

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
