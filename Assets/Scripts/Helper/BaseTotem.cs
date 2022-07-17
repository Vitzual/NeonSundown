using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTotem : Drone
{
    // Sound that plays when entering totems range
    public AudioClip totemSound;

    // Base totem variables
    public Transform radius;
    public float targetRadius;
    public Vector3 scaleSpeed;
    public float maxDistance;
    protected float cooldown;

    // Within range flag
    protected bool shipInsideRange;

    // On start, play animation
    public override void CustomUpdate()
    {
        if (Dealer.isOpen) return;

        if (radius.localScale.x < targetRadius)
            radius.localScale += scaleSpeed * Time.deltaTime;
    }

    // On collision with enemy, apply damage
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        AudioPlayer.Play(totemSound, false, 1f, 1f, false, 0.6f);
        shipInsideRange = true;
    }

    // On collision with enemy, apply damage
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        // Get the enemy component
        AudioPlayer.Play(totemSound, false, 0.8f, 0.8f, false, 0.6f);
        shipInsideRange = false;
    }
}
