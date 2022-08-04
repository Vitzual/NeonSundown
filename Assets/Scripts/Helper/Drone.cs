using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Helper
{
    // Drone variables
    public float damage;
    public float movementSpeed;
    public float rotationSpeed;
    public float range = 35f;
    protected float directionCooldown;

    // Move totem around randomly
    public void FixedUpdate()
    {
        // Check if dealer is open
        if (Dealer.isOpen) return;
        else Move();
    }

    // Moves the drone
    public virtual void Move()
    {
        if (ship == null) return;

        // Check position relative to player
        if (Vector2.Distance(transform.position, ship.transform.position) > range)
        {
            // Rotate towards the object
            if (rotationSpeed != 60f) rotationSpeed = 60f;
            float angle = Mathf.Atan2(ship.transform.position.y - transform.position.y,
                ship.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        else
        {
            // Rotate randomly
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            // Check direction cooldown
            if (directionCooldown <= 0f)
            {
                if (Random.Range(0f, 1f) > 0.5f) rotationSpeed = Random.Range(-80f, -40f);
                else rotationSpeed = Random.Range(40f, 80f);
                directionCooldown = 3f;
            }
            else directionCooldown -= Time.deltaTime;
        }

        // Move forward
        transform.position += transform.up * movementSpeed * Time.fixedDeltaTime;
    }
}
