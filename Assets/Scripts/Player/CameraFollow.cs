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
    public static bool freecam;
    float horizontal;
    float vertical;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(Keybinds.sprint))
            speed = 250f;
        else if (Input.GetKeyUp(Keybinds.sprint))
            speed = 100f;

        if (freecam) CheckMovementInput();
    }

    void FixedUpdate()
    {
        // Check if freecam is enabled
        if (freecam) body.velocity = new Vector2(horizontal * speed, vertical * speed);
        else
        {
            // Check if target is null
            if (target == null) return;

            Vector3 goalPos = target.position;
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
        }
    }

    // Checks for movement input
    private void CheckMovementInput()
    {
        vertical = 0;
        vertical += Input.GetKey(Keybinds.move_up) ? 1 : 0;
        vertical -= Input.GetKey(Keybinds.move_down) ? 1 : 0;

        horizontal = 0;
        horizontal += Input.GetKey(Keybinds.move_right) ? 1 : 0;
        horizontal -= Input.GetKey(Keybinds.move_left) ? 1 : 0;
    }

    public void SetTarget(Transform target)
    {
        // Todo: Validate target is a targetable target
        this.target = target;
    }
}
