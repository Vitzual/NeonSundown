using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Weapon
{
    // Turret aiming thing
    private bool hasTarget = false;
    private Material material;

    // Setup the turret
    public void Setup(WeaponData weapon, float cooldown, Transform target, Material material = null)
    {
        this.material = material;
        hasTarget = target != null;

        if (!hasTarget)
            transform.eulerAngles = new Vector3(0, 0, 90);

        this.weapon = weapon;
        this.cooldown = cooldown;
        this.target = target;

        if (hasTarget) SetupStats();
    }

    public override void Use()
    {
        // Check if has target
        if (!hasTarget) return;

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
        BulletHandler.active.CreateBullet(this, weapon, transform.position, transform.rotation, 
            1, weapon.bloom, weapon.bulletSize, material, false, false, false, null);
    }
}
