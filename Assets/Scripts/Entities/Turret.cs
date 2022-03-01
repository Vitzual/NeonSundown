using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Weapon
{
    // Setup the turret
    public void Setup(WeaponData weapon, float cooldown, Transform target)
    {
        this.weapon = weapon;
        this.cooldown = cooldown;
        this.target = target;
        SetupStats();
    }

    public override void Use()
    {
        // Rotate to the target
        float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
            target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = targetRotation;

        // Calculate cooldown
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            cooldown = Random.Range(1f, 2f);
            Fire();
        }
    }

    // Create bullet
    public void Fire()
    {
        // Creat the bullet
        BulletHandler.active.CreateBullet(this, weapon, transform.position, transform.rotation, 1);
    }
}
