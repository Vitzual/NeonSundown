using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerDrone : Drone
{
    // Runtime references
    public Entity target;

    // Set damage
    public override void Setup(Ship ship, HelperData data)
    {
        // dumb. this should be its own SO
        damage = ship.shipData.droneDamage;
        base.Setup(ship, data);
    }

    // Move drone towards target
    public override void Move()
    {
        // Check if target is null
        if (target == null) base.Move();

        // If target not null, move
        else
        {
            float angle = Mathf.Atan2(target.transform.position.y - transform.position.y,
                target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

            // Rotate to target
            transform.rotation = Quaternion.RotateTowards(transform.rotation,
                targetRotation, ship.rotateSpeed * Time.deltaTime);

            // Move forward
            transform.position += transform.up * movementSpeed * Time.fixedDeltaTime;
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Get enemy component
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        // Check if enemy is not null
        if (enemy != null)
        {
            enemy.Damage(damage, -1500f);
            Knockback(-1500f, enemy.transform.position);
        }
        else
        {
            // Check if crystal
            Crystal crystal = collision.collider.GetComponent<Crystal>();

            // Check if crystal is not null, and destroy
            if (crystal != null) crystal.Destroy();
        }
    }
}
