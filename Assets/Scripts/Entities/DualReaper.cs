using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualReaper : Weapon
{
    public Scythe scytheOne, scytheTwo;

    // Override default setup
    public override void Setup(WeaponData data, Transform target = null)
    {
        scytheOne.Setup(data, transform);
        scytheTwo.Setup(data, transform);
        base.Setup(data, target);
    }

    // Rotates around the player
    public override void Use()
    {
        scytheOne.Use();
        scytheTwo.Use();
    }
}
