using System;
using UnityEngine;

// Handles anything player-input related

public class Controller : MonoBehaviour
{
    // Controller object
    public GameObject _controller;
    public static GameObject controller;
    private Vector3 lastMousePos;

    // Transform that rotates
    public Transform rotator;

    // Dashing sound
    public AudioClip dashSound;

    // GameObject child transforms
    private Rigidbody2D body;

    // Base stat variables
    public float dashDamage = 3f;
    public float dashTimeSlowdown = 0.8f;
    public float dashTimeRecoveryValue = 0.1f;

    // Movement variables
    public float moveSpeed = 5f;
    public float dashSpeed = 25f;
    public float dashTimer = 0.5f;
    public float dashCooldown = 1f;
    public float dashFatigue = 0.1f;

    // Dash flag
    public bool canRotate = true;
    private bool isDashing = false;
    private bool dashOnCooldown = false;
    private bool dashQuickReset = false;

    // Internal input measurements
    private float horizontal;
    private float vertical;
    private float dash;
    private float speed;

    // Cursor variables
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Controller flag
    private bool isControllerConnected;

    // Called on start
    void Start()
    {
        // Start for everyone
        body = GetComponent<Rigidbody2D>();
        lastMousePos = Vector2.zero;
        controller = _controller;
    }

    // Normal frame update
    void Update()
    {
        // Check if controller connected
        isControllerConnected = Input.GetJoystickNames().Length > 0;

        // Check if something is open
        if (Dealer.isOpen)
        {
            vertical = 0;
            horizontal = 0;
            body.velocity = new Vector2(0, 0);
            return;
        }

        // Checks for keyboard input
        CheckMovementInput();
        if (canRotate) RotateToMouse();
    }

    // Physics update for handling movement calculations 
    private void FixedUpdate()
    {
        // Set speed
        if (isDashing && speed > moveSpeed) 
            speed -= dashFatigue;
        else speed = moveSpeed;

        // Set rigidbody velocity
        body.velocity = new Vector2(horizontal * speed, vertical * speed);
    }

    // Checks for movement input
    private void CheckMovementInput()
    {
        // Horizontal Movement
        horizontal = 0;
        horizontal += Input.GetAxis("L-Stick X");
        horizontal += Input.GetKey(Keybinds.move_right) ? 1 : 0;
        horizontal -= Input.GetKey(Keybinds.move_left) ? 1 : 0;
        horizontal = Mathf.Clamp(horizontal, -1, 1);

        // Vertical Movement
        vertical = 0;
        vertical += Input.GetAxis("L-Stick Y");
        vertical += Input.GetKey(Keybinds.move_up) ? 1 : 0;
        vertical -= Input.GetKey(Keybinds.move_down) ? 1 : 0;
        vertical = Mathf.Clamp(vertical, -1, 1);

        // Check if dashing
        if (isDashing)
        {
            dash -= Time.deltaTime;
            if (dash <= 0)
            {
                isDashing = false;
                dash = dashCooldown;
                dashOnCooldown = !dashQuickReset;
            }
        }

        // Check if dash on cooldown
        else if (dashOnCooldown)
        {
            dash -= Time.deltaTime;
            if (dash <= 0) dashOnCooldown = false;
        }

        // Check if dash pressed
        else if (Input.GetKey(Keybinds.dash) || Input.GetKey(KeyCode.Joystick1Button4))
        {
            isDashing = true;
            dash = dashTimer;
            speed = dashSpeed;
            dashQuickReset = false;
        }
    }

    // Rotates the players head towards the mouse
    private void RotateToMouse()
    {
        // Controller input
        float horizontal = Input.GetAxis("R-Stick X");
        float vertical = Input.GetAxis("R-Stick Y");

        // Check horizontal and vertical
        if (horizontal != 0 || vertical != 0)
        {
            if (!controller.activeSelf) controller.SetActive(true);
            controller.transform.localPosition = new Vector2(horizontal * 40, vertical * 40);
            float angle = Mathf.Atan2(controller.transform.localPosition.y, controller.transform.localPosition.x) * Mathf.Rad2Deg;
            rotator.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        
        // Get mouse pos
        Vector3 mousePos = Input.mousePosition;
        if (mousePos != lastMousePos)
        {
            if (controller.activeSelf) controller.SetActive(false);

            lastMousePos = mousePos;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(rotator.position);

            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            rotator.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    //public void OnMouseEnter() { Cursor.SetCursor(cursorTexture, hotSpot, cursorMode); }
    //public void OnMouseExit() { Cursor.SetCursor(null, Vector2.zero, cursorMode); }
}