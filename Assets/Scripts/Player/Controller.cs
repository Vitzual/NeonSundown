using UnityEngine;

// Handles anything player-input related

public class Controller : MonoBehaviour
{
    // Transform that rotates
    public Transform rotator;

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
    private bool isDashing = false;
    private bool dashOnCooldown = false;
    private bool dashQuickReset = false;

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
        else if (Input.GetKey(Keybinds.sprint))
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
        Vector3 mousePos = Input.mousePosition;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(rotator.position);

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        rotator.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // On collision with enemies
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Grab the enemy component attached to the enemy
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        // Check if the collision was indeed an enemy
        if (enemy != null)
        {
            if (isDashing)
            {
                if (enemy.IsDashResistant())
                {
                    // ow
                }
                else
                {
                    // Damage the enemy
                    enemy.Damage(Deck.CalculateStat(Stat.DashDamage, dashDamage));

                    // Reset dash
                    isDashing = true;
                    dash = dashTimer / 2;
                    speed = dashSpeed;
                    dashQuickReset = true;

                    // Slow down time
                    // Time.timeScale = 0.1f;
                    // Time.fixedDeltaTime = 0.002f;
                }
            }
        }
    }
}