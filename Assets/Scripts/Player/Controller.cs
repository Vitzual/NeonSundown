using UnityEngine;

// Handles anything player-input related

public class Controller : MonoBehaviour
{
    // GameObject child transforms
    private Rigidbody2D body;

    // Movement variables
    public float moveSpeed = 5f;
    public float dashSpeed = 25f;
    public float dashTimer = 0.5f;
    public float dashCooldown = 1f;
    public float dashFatigue = 0.1f;

    // Dash flag
    private bool isDashing = false;
    private bool dashOnCooldown = false;

    // Internal input measurements
    private float horizontal;
    private float vertical;
    private float dash;
    private float speed;

    // Called on start
    void Start()
    {
        // Start for everyone
        body = GetComponent<Rigidbody2D>();
    }

    // Normal frame update
    void Update()
    {
        // Checks for keyboard input
        CheckMovementInput();
        RotateToMouse();
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
        // Vertical Movement
        vertical = 0;
        vertical += Input.GetKey(Keybinds.move_up) ? 1 : 0;
        vertical -= Input.GetKey(Keybinds.move_down) ? 1 : 0;

        // Horizontal Movement
        horizontal = 0;
        horizontal += Input.GetKey(Keybinds.move_right) ? 1 : 0;
        horizontal -= Input.GetKey(Keybinds.move_left) ? 1 : 0;

        // Check if dashing
        if (isDashing)
        {
            dash -= Time.deltaTime;
            if (dash <= 0)
            {
                isDashing = false;
                dashOnCooldown = true;
                dash = dashCooldown;
            }
        }

        // Check if dash on cooldown
        else if (dashOnCooldown)
        {
            dash -= Time.deltaTime;
            if (dash <= 0) dashOnCooldown = false;
        }

        // Check if dash pressed
        else if (Input.GetKey(Keybinds.sprint))
        {
            isDashing = true;
            dash = dashTimer;
            speed = dashSpeed;
        }
    }

   // Rotates the players head towards the mouse
    private void RotateToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}