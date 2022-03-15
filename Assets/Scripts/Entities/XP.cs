using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    // The target in question
    public float startDistance = 5f;
    [HideInInspector] public bool isMoving, isStarting, isInactive;
    [HideInInspector] public float speed, value, timer;
    [HideInInspector] public Vector2 startPos;

    // Start method
    public void Setup(Vector2 startPos, float value)
    {
        this.startPos = startPos;
        this.value = value;
        isMoving = false;
        isStarting = true;
        isInactive = false;
        timer = 10;
        speed = 10;
    }

    // On collision with player
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInactive)
            isMoving = true;
    }
}
