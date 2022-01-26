using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : Weapon
{
    // Internal variables
    protected float cooldown;

    // Set the SO reference
    public override void Setup(WeaponData data, Transform target = null)
    {
        cooldown = data.cooldown;
        base.Setup(data, target);
    }

    // Shoots projectiles
    public override void Use()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            BulletHandler.active.CreateBullet(weaponData, transform.position, transform.rotation);
            cooldown = weaponData.cooldown;
        }
    }
}
