using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserchain : Boss
{
    public enum Action
    {
        Starting,
        Moving,
        Firing,
        Cooldown
    }

    public Action action = Action.Starting;
    public Transform rotator;
    public float rotationSpeed = 10f;
    public float moveSpeed = 15f;
    public float cooldown = 5f;

    public override void Setup()
    {
        transform.position = new Vector2(-200, 0);
        base.Setup();
    }

    public void FixedUpdate()
    {
        switch(action)
        {
            // Move towards center of screen
            case Action.Starting:
                rotator.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, moveSpeed * Time.deltaTime);
                if (transform.position.x >= -0.5f)
                {
                    cooldown = 5f;
                    action = Action.Cooldown;
                }
                break;

            // On cooldown (pick random action after)
            case Action.Cooldown:
                cooldown -= Time.deltaTime;
                if (cooldown <= 0)
                {
                    if (Random.Range(0f, 1f) > 0.5f)
                    {
                        action = Action.Moving;
                        cooldown = 10f;
                    }
                    else
                    {
                        action = Action.Firing;
                        cooldown = 10f;
                    }
                }
                break;

            // Firing lasers
            case Action.Firing:
                cooldown -= Time.deltaTime;
                if (cooldown <= 0)
                {
                    cooldown = 5f;
                    action = Action.Cooldown;
                }
                else
                {

                }
                break;

            // Firing lasers
            case Action.Moving:
                cooldown -= Time.deltaTime;
                if (cooldown <= 0)
                {
                    cooldown = 5f;
                    action = Action.Cooldown;
                }
                else
                {

                }
                break;
        }
    }
}
