using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : Weapon
{
    // Internal cooldown
    protected float weaponCooldown = 0;
    protected int buckshots = 0, buckshotCounter = 0;
    private float vertical, horizontal;
    
    // Set the SO reference
    public override void Setup(WeaponData data, Transform target = null)
    {
        base.Setup(data, target);
    }
    
    // Shoots projectiles
    public override void Use()
    {
        if (!weapon.randomDirection && (Controller.vertical != 0 || Controller.horizontal != 0))
        {
            vertical = Controller.vertical;
            horizontal = Controller.horizontal;
        }

        weaponCooldown -= Time.deltaTime;
        if (weaponCooldown <= 0)
        {
            if (weapon.randomDirection) 
                transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            else 
            {
                float angle = Mathf.Atan2(vertical, horizontal) * Mathf.Rad2Deg;
                transform.eulerAngles = new Vector3(0, 0, angle);
            }

            int bulletsToFire = (int)bullets;
            if (buckshots > 0)
            {
                if (buckshotCounter == 0)
                {
                    bulletsToFire += buckshots;
                    buckshotCounter = 4;
                }
                else buckshotCounter -= 1;
            }

            for (int i = 0; i < bulletsToFire; i++)
            {
                BulletHandler.active.CreateBullet(this, weapon, transform.position, transform.rotation, weapon.bullets,
                    weapon.bloom, size, weapon.material, false, explosiveRounds);
            }

            for (int i = 0; i < reverseShots; i++)
            {
                BulletHandler.active.CreateBullet(this, weapon, transform.position, Quaternion.Inverse(transform.rotation),
                    weapon.bullets, weapon.bloom, size, weapon.material, false, explosiveRounds);
            }

            if (cooldown < 0.05f) weaponCooldown = 0.05f;
            else weaponCooldown = cooldown;
        }
    }

    public override void UpdateStat(Stat type)
    {
        switch (type)
        {
            case Stat.Bullets:
                bullets = (Deck.CalculateStat(type, weapon.bullets)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            case Stat.Buckshot:
                buckshots = (int)((Deck.CalculateStat(type, 0) 
                    + GetAdditions(type)) * GetMultiplier(type));
                break;
            case Stat.Splitshot:
                splitshots = (Deck.CalculateStat(type, 0)
                    + GetAdditions(type)) * GetMultiplier(type);
                break;
            default:
                base.UpdateStat(type);
                break;
        }
    }
}
