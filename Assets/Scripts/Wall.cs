using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Helper
{
    public AudioClip sound;
    public Transform radius;
    private bool moveBack = false;

    // On start, play animation
    public void FixedUpdate()
    {
        if (Dealer.isOpen) return;

        // Check position relative to player
        if (!moveBack && Vector2.Distance(transform.position, ship.transform.position) > 45f) moveBack = true;

        // If move back, move back
        if (moveBack && Vector2.Distance(transform.position, ship.transform.position) > 35f)
            transform.position = Vector2.MoveTowards(transform.position, ship.transform.position, 30f * Time.deltaTime);
        else moveBack = false;
    }

    // On collision with bullet, destroy it
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null) bullet.Destroy();
    }

    // On collision with bullet, destroy it
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the enemy component
        Bullet bullet = collision.collider.GetComponent<Bullet>();
        if (bullet != null) bullet.Destroy();
    }
}
