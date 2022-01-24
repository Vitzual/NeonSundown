using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Target
    public Transform target;

    // Variables
    public float damage;
    public float speed;
    public float pierce;
    public bool tracking;

    public virtual void Setup(float damage, float speed, float pierce, bool tracking)
    {
        this.damage = damage;
        this.speed = speed;
        this.pierce = pierce;
        this.tracking = tracking;
    }

    public virtual void Move()
    {
        if (tracking && target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
        else transform.position += transform.up * speed * Time.deltaTime;
    }
}
