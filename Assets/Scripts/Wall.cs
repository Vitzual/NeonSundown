using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Helper
{
    public AudioClip sound;
    public Transform radius;
    public float targetRadius;
    public Vector3 scaleSpeed;

    // On start, play animation
    public override void CustomUpdate()
    {
        if (Dealer.isOpen) return;

        if (radius.localScale.x < targetRadius)
            radius.localScale += scaleSpeed * Time.deltaTime;
    }

    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null) bullet.Destroy();
    }
}
