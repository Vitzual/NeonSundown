using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    // The target in question
    public float startDistance = 5f;
    private XPHandler.XPInstance xpReference;

    // Start method
    public void Setup(Vector2 startPos)
    {
        xpReference = XPHandler.active.Register(transform, startPos);
    }

    // On collision with player
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Controller>() != null)
        {
            if (xpReference != null)
                xpReference.isMoving = true;
        }
    }
}
