using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telefrag : Teleport
{
    public override void Use()
    {
        if (cooldown <= 0 && !Dealer.isOpen)
        {
            base.Use();
            ExplosiveHandler.CreateExplosion(ship.transform.position, 15f, 50f, -2500f, ship.border.material);
        }
    }
}
