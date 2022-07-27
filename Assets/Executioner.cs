using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : Enemy
{
    // Seperate rotating saw
    public Transform saw;

    // Saw variable stats
    public float sawRotateSpeed;

    // Override move method for saw rotation
    public override void Move()
    {
        // Rotate the saw seperately from base transform
        saw.Rotate(saw.forward, sawRotateSpeed * Time.deltaTime);

        // Call base
        base.Move();
    }
}
