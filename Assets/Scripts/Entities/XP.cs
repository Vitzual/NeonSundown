using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    // The target in question
    public float startDistance = 5f;
    private float amount = 1;
    private XPHandler.XPInstance xpReference;

    // Start method
    public void Setup(Vector2 startPos, float value)
    {
        xpReference = XPHandler.active.Register(value, transform, startPos);
        this.amount = value;
    }

    // On collision with player
    public void OnTriggerEnter2D(Collider2D collision)
    {
        xpReference.isMoving = true;
    }
}
