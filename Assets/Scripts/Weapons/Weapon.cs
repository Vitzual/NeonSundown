using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Target transform
    protected Transform target;

    // Weapon data
    protected WeaponData weapon;

    // Set the weapon data
    public virtual void Setup(WeaponData data, Transform target = null)
    {
        this.target = target;
        weapon = data;
    }

    public virtual void Use()
    {
        // Do something
    }
}
