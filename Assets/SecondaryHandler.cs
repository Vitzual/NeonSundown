using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryHandler : MonoBehaviour
{
    // Secondary variables
    protected SecondaryData data;
    protected Secondary instance;

    // Secondary variables
    protected float cooldown;

    // Equip secondary
    public void Equip(SecondaryData data)
    {
        // Reset data and cooldown
        this.data = data;
        cooldown = data.cooldown;
        

    }

    // Use secondary
    public void Use()
    {
        // Check if cooldown finished
        if (cooldown > 0f) return;
        cooldown = data.cooldown;

    }
}
