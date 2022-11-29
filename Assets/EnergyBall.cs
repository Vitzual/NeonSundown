using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : Bullet
{
    public Bullet miniBullet;
    public float cooldown;

    public override void Move()
    {
        base.Move();

        if (cooldown <= 0f)
        {
            cooldown = 5f;

            
        }
    }
}
