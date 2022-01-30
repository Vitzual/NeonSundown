using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : Weapon
{
    // Internal cooldown
    protected float weaponCooldown = 0;

    // Set the SO reference
    public override void Setup(WeaponData data, Transform target = null)
    {
        base.Setup(data, target);
    }

    // Shoots projectiles
    public override void Use()
    {
        weaponCooldown -= Time.deltaTime;
        if (weaponCooldown <= 0)
        {
            if (weapon.randomDirection)
                transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));

            BulletHandler.active.CreateBullet(this, weapon, transform.position, transform.rotation);
            weaponCooldown = cooldown;
        }
    }
}
