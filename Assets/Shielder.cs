using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shielder : Enemy
{
    // Shield stats
    public GameObject shield;
    public float shieldHealth;
    private bool shieldActive;

    // Damage entity
    public override void Damage(float amount)
    {
        // Damage shield if active
        if (shieldActive)
        {
            shieldHealth -= amount;
            if (shieldHealth <= 0)
            {
                shieldActive = false;
                shield.SetActive(false);
            }
        }
        else base.Damage(amount);
    }
}
