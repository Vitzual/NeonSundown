using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDrone : Drone
{
    // Auto shooter variables
    public AutoShooter projectileOne, projectileTwo;
    public WeaponData weaponOne, weaponTwo;

    // Setup the projectile drone
    public void Start()
    {
        projectileOne.Setup(weaponOne);
        projectileTwo.Setup(weaponTwo);
    }

    // Override custom update
    public override void CustomUpdate()
    {
        projectileOne.Use();
        projectileTwo.Use();
    }
}
