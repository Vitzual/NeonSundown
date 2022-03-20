using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObj : MonoBehaviour
{
    // On collision with enemy, apply damage
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Get the enemy component
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null) bullet.Destroy();
    }
}
