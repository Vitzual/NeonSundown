using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileDrone : Drone
{
    // Auto shooter variables
    public AutoShooter projectileOne, projectileTwo;
    public WeaponData weaponOne, weaponTwo;
    protected ProjectileDrone droneOne, droneTwo;

    // Setup the projectile drone
    public void Start()
    {
        projectileOne.Setup(weaponOne);
        projectileTwo.Setup(weaponTwo);
    }

    // Virtual setup method
    public override void Setup(Ship ship, HelperData data)
    {
        this.ship = ship;
        this.data = data;

        droneOne = Instantiate(this, new Vector3(
            transform.position.x + 5f, 
            transform.position.y + 5f, 
            transform.position.z), 
            Quaternion.identity);
        droneOne.CloneSetup(ship, data);

        droneTwo = Instantiate(this, new Vector3(
            transform.position.x - 5f, 
            transform.position.y - 5f, 
            transform.position.z), 
            Quaternion.identity);
        droneTwo.CloneSetup(ship, data);
    }

    public void CloneSetup(Ship ship, HelperData data)
    {
        this.ship = ship;
        this.data = data;
    }

    // Override custom update
    public override void CustomUpdate()
    {
        projectileOne.Use();
        projectileTwo.Use();

        droneOne.CloneUpdate();
        droneTwo.CloneUpdate();
    }

    public void CloneUpdate()
    {
        projectileOne.Use();
        projectileTwo.Use();
    }

    // Moves the drone
    public override void Move()
    {
        if (ship == null) return;

        // Check position relative to player
        if (Vector2.Distance(transform.position, ship.transform.position) > range)
        {
            // Rotate towards the object
             float angle = Mathf.Atan2(ship.transform.position.y - transform.position.y,
                ship.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 250f * Time.deltaTime);
        }

        else
        {
            // Rotate randomly
            transform.Rotate(Vector3.forward, 250f * Time.deltaTime);
        }

        // Move forward
        transform.position += transform.up * movementSpeed * Time.fixedDeltaTime;
    }
}
