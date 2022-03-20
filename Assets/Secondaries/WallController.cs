using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : Secondary
{
    // Wall objects
    public WallObj newWall;
    public WallObj wallObj;
    public float redeployCooldown;

    // Get the audio source
    public override void Setup(Ship ship, SecondaryData data)
    {
        redeployCooldown = data.cooldown;
        base.Setup(ship, data);
    }

    // Teleport the ship to mouse cursor
    public override void Use()
    {
        if (cooldown <= 0 && !Dealer.isOpen)
        {
            // Reset cooldown
            cooldown = redeployCooldown;

            // Create particle at location
            if (wallObj != null) Destroy(wallObj.gameObject);

            // Create totem at mouse location
            if (Controller.controller.activeSelf) wallObj = Instantiate(newWall, Controller.controller.transform.position, Quaternion.identity);
            else wallObj = Instantiate(newWall, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        }
    }

    // Override destroy
    public override void Destroy()
    {
        if (wallObj != null) 
            Destroy(wallObj.gameObject);
        base.Destroy();
    }
}
