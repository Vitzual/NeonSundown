using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secondary : MonoBehaviour
{
    // Ship instance
    protected SecondaryData data;
    protected Ship ship;
    protected float cooldown;

    // Virtual setup method
    public virtual void Setup(Ship ship, SecondaryData data) 
    { 
        this.ship = ship;
        this.data = data;
        cooldown = ship.cooldown;
    }

    // Virtual update method
    public virtual void Update()
    {
        if (!Dealer.isOpen && cooldown > 0)
            cooldown -= Time.deltaTime;
    }

    // Virtual use method
    public virtual void Use()
    {
        if (cooldown <= 0)
            cooldown = ship.cooldown;
    }
}
