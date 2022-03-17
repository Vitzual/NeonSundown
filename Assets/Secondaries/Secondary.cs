using UnityEngine;

public class Secondary : MonoBehaviour
{
    // Ship instance
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
    public virtual void Upgrade()
    {
        Debug.Log("Upgrading " + data.name);
        level += 1;
        cooldown = 0;
    }

    // Returns a formatted string on the upgrade info
    public virtual string GetUpgradeString()
    {
        if (level < data.levels.Count)
            return data.levels[level].description;
        else return "LEVEL MAX";
    }

    // Returns a stat
    public virtual float GetStat(Stat stat)
    {
        if (stat == Stat.Cooldown)
            return cooldown;
        else return -1;
    }
}
