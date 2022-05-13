using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dualwing : Boss
{
    // On collision
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the other enemy component
        Ship ship = collision.GetComponent<Ship>();

        // If is bullet, invoke on hit method
        if (ship != null) ship.Damage(enemy.GetData().damage);
    }
}
