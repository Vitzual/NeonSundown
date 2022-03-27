using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razorsaw : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null) bullet.ReverseBullet();
        else
        {
            Ship ship = collision.GetComponent<Ship>();
            if (ship != null) ship.Damage(10f);
        }
    }
}
