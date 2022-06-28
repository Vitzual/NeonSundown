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
            else 
            {
                float angle = Mathf.Atan2(Controller.vertical, Controller.horizontal) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle);
            }

            BulletHandler.active.CreateBullet(this, weapon, transform.position, transform.rotation,
                weapon.bullets, weapon.bloom, weapon.material, false, explosiveRounds, true);
            if (cooldown < 0.05f) weaponCooldown = 0.05f;
            else weaponCooldown = cooldown;
        }
    }
}
