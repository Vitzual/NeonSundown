using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{
    // Parent reference
    public Laser laser;

    // On thing
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if enemy laser
        if (laser.enemyLaser)
        {
            Ship ship = collision.GetComponent<Ship>();
            if (ship != null) ship.Damage(5f);
        }

        // If not enemy laser, check for enemy
        else
        {
            Entity entity = collision.GetComponent<Entity>();
            if (entity != null) laser.OnHit(entity);
        }
    }
}
