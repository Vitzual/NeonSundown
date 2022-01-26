using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Target transform
    protected Transform target;

    // Weapon data
    protected WeaponData weaponData;

    // Set the weapon data
    public virtual void Setup(WeaponData data, Transform target = null)
    {
        this.target = target;
        weaponData = data;
    }

    public virtual void Use()
    {
        // Do something
    }
}
