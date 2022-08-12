using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    // List of entities in laser
    protected List<Entity> entities = new List<Entity>();

    // Laser descrease speed
    public float laserSpeed;
    public bool constantDamage, enemyLaser;
    public Transform rotationPoint;
    public Transform laserPoint;

    public virtual void SetupLaser(Transform parent, float width, float length, float bloom)
    {
        // Set rotation
        rotationPoint.SetParent(parent.transform);
        rotationPoint.localPosition = Vector3.zero;
        rotationPoint.rotation = parent.transform.rotation;
        laserPoint.localScale = new Vector2(width, length);
        laserPoint.localPosition = new Vector2(0, length / 2);

        // Offset rotation slightly
        Vector3 rotationOffset = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            transform.eulerAngles.z + bloom);
        rotationPoint.eulerAngles = rotationOffset;
    }

    // Lower laser size
    public override void Move()
    {
        if (transform.localScale.x > 0.2f)
        {
            transform.localScale = new Vector2(transform.localScale.x -
                (Time.deltaTime * laserSpeed), transform.localScale.y);
        }
        else Destroy();
    }

    // Override destroy
    public override void Destroy()
    {
        // Check if enemy laser
        if (enemyLaser) return;
        Destroy(gameObject);
    }

    public virtual void AddEntity(Entity entity)
    {
        if (!entities.Contains(entity))
            entities.Add(entity);
    }

    public virtual void RemoveEntity(Entity entity)
    {
        if (entities.Contains(entity))
            entities.Remove(entity);
    }
}
