using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scattershot : AutoShooter
{
    // Scattershot flag
    private bool verticalAdjustment = true;

    // Shoots projectiles
    public override void Use()
    {
        weaponCooldown -= Time.deltaTime;
        if (weaponCooldown <= 0)
        {
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

            // Adjust vertical shots
            float adjustment = 0f;
            if (verticalAdjustment) adjustment = 45f;
            verticalAdjustment = !verticalAdjustment;

            for (int r = 0; r < 4; r++)
            {
                // Get rotation
                Quaternion rotation = new Quaternion();

                // Calculate spread
                float spread = 0;
                if (bulletsToFire > 1) spread -= (2.5f * bulletsToFire) - 2.5f;

                // Iterate through bullet amount
                for (int i = 0; i < bulletsToFire; i++)
                {
                    // Calculate rotation
                    switch (r)
                    {
                        case 0: rotation.eulerAngles = new Vector3(0, 0, 0 + spread + adjustment); break;
                        case 1: rotation.eulerAngles = new Vector3(0, 0, 90 + spread + adjustment); break;
                        case 2: rotation.eulerAngles = new Vector3(0, 0, 180 + spread + adjustment); break;
                        case 3: rotation.eulerAngles = new Vector3(0, 0, 270 + spread + adjustment); break;
                    }

                    // Instantiate bullet
                    BulletHandler.active.CreateBullet(this, weapon, transform.position, rotation, (int)bullets,
                        weapon.bloom, size, weapon.material, false, explosiveRounds);

                    // Update spread
                    spread += 5f;
                }
            }

            if (cooldown < 0.05f) weaponCooldown = 0.05f;
            else weaponCooldown = cooldown;
        }
    }
}
