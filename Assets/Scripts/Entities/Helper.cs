using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    // Ship instance
    protected Ship ship;
    [HideInInspector] public HelperData data;
    [HideInInspector] public int level;

    // Virtual setup method
    public virtual void Setup(Ship ship, HelperData data)
    {
        this.ship = ship;
        this.data = data;
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
    }

    // Returns a formatted string on the upgrade info
    public virtual string GetUpgradeString()
    {
        if (level < data.levels.Count)
            return data.levels[level].description;
        else return "LEVEL MAX";
    }

    // Overrides the get stat function
    public virtual float GetStat(Stat stat)
    {
        return 0;
    }
}
