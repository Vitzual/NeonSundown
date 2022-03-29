using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserchain : Boss
{
    public enum Action
    {
        Starting,
        Firing,
        Cooldown
    }

    public Action action = Action.Starting;
    public Transform rotator;
    public Laser laserOne, laserTwo;
    public float rotationSpeed = 10f;
    public float laserSpeed = 10f;
    public float moveSpeed = 15f;
    public float cooldown = 5f;

    public override void Setup()
    {
        transform.position = new Vector2(-200, 0);
        laserOne.SetDamage(10f);
        laserTwo.SetDamage(10f);
        base.Setup();
    }

    public void FixedUpdate()
    {
        // Check if dealer is open
        if (Dealer.isOpen) return;

        switch(action)
        {
            // Move towards center of screen
            case Action.Starting:
                rotator.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, moveSpeed * Time.deltaTime);
                if (transform.position.x >= -0.5f)
                {
                    cooldown = 2f;
                    action = Action.Cooldown;
                }
                break;

            // On cooldown (pick random action after)
            case Action.Cooldown:
                rotator.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                cooldown -= Time.deltaTime;
                if (cooldown <= 0) action = Action.Firing;
                break;

            // Firing lasers
            case Action.Firing:
                rotator.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                if (laserOne.transform.localScale.x <= 250f)
                {
                    laserOne.transform.localScale = new Vector3(laserOne.transform.localScale.x + (laserSpeed * Time.deltaTime),
                        laserOne.transform.localScale.y, laserOne.transform.localScale.z);
                    laserTwo.transform.localScale = laserOne.transform.localScale;
                }
                break;
        }
    }
}
