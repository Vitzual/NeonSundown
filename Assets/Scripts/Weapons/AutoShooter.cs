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
            if (weapon.randomDirection)
                transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));

            BulletHandler.active.CreateBullet(weapon, transform.position, transform.rotation);
            cooldown = weapon.cooldown;
        }
    }
}
