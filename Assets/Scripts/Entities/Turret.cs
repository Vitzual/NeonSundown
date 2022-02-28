using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public float cooldown;
    protected WeaponData weapon;
    protected Bullet bullet;
    protected Transform target;

    // Setup the turret
    public void Setup(WeaponData weapon, Bullet bullet, float cooldown, Transform target)
    {
        this.weapon = weapon;
        this.bullet = bullet;
        this.cooldown = cooldown;
        this.target = target;
    }

    // Create bullet
    public void Fire()
    {
        // Creat the bullet
        BulletHandler.active.CreateBullet(null, weapon, transform.position, transform.rotation, 1);
    }
}
