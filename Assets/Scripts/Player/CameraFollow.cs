using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Cam variables
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;

    // Speed modifier
    private Vector3 velocity = Vector3.zero;

    // Movement variables
    private Rigidbody2D body;
    public float speed;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Check if target is null
        if (target == null) return;

        Vector3 goalPos = target.position;
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }

    public void SetTarget(Transform target)
    {
        // Todo: Validate target is a targetable target
        this.target = target;
    }
}
