using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualReaper : Weapon
{
    public List<Scythe> scythes;

    // Override default setup
    public override void Setup(WeaponData data, Transform target = null)
    {
        foreach (Scythe scythe in scythes)
            scythe.Setup(data, target);

        base.Setup(data, target);
    }
    
    // Rotates around the player
    public override void Use()
    {
        foreach (Scythe scythe in scythes)
            scythe.Use();
    }

    // Override upgrade call
    public override void Upgrade(UpgradeData upgrade, int quality)
    {
        foreach (Scythe scythe in scythes)
            scythe.Upgrade(upgrade, quality);
    }

    public override void UpdateStat(Stat type)
    {
        return;
    }

    public override void Destroy()
    {
        foreach (Scythe scythe in scythes)
            scythe.Destroy();
        base.Destroy();
    }
}
