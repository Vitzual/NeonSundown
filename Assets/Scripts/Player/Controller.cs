using UnityEngine;
using UnityEngine.InputSystem;

// Handles anything player-input related

public class Controller : MonoBehaviour
{
    // Reference to ship
    private Ship ship;
    
    // Controller associated with the player
    public static bool isControllerConnected;
    public GameObject controllerIcon;

    // Controller object
    public GameObject _controller;
    public static GameObject controller;

    // Transform that rotates
    public Transform rotator;

    // Dashing sound
    public AudioClip dashSound;
    public AudioSource audioSource;

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
    private float chargeAmount = 0f;
    private float moveSpeedHolder = 5f;

    // Dash flag
    public bool canRotate = true;
    [HideInInspector]
    public bool isDashing = false;
    public bool isChargeShip = false, charging = false;
    private bool dashOnCooldown = false;
    private bool dashQuickReset = false;
    private bool shipOverridesDash = false;

    // Internal input measurements
    public static float horizontal;
    public static float vertical;
    public static float speed;
    private float dash;

    // Cursor variables
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    protected Vector2 lastMousePos = Vector2.zero;

    // Called on start
    void Start()
    {
        // Reset cursor lock state
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Cursor.visible = true;

        // Start for everyone
        body = GetComponent<Rigidbody2D>();
        ship = GetComponent<Ship>();
        controller = _controller;
        horizontal = 0;
        vertical = 0;
    }

    // Normal frame update
    void Update()
    {
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
        Vector2 movement = CIN._action_move.ReadValue<Vector2>();
        horizontal = Mathf.Clamp(movement.x, -1, 1);
        vertical = Mathf.Clamp(movement.y, -1, 1);

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
        else if (CIN._action_sprint.IsPressed())
        {
            // Check if ship overrides dash
            if (shipOverridesDash) ship.DashOverride();

            // Check if dash ship
            if (isChargeShip)
            {
                if (!charging) moveSpeedHolder = moveSpeed;
                if (chargeAmount < 1)
                {
                    chargeAmount += Time.deltaTime;
                    moveSpeed *= (1 - chargeAmount);
                }
                charging = true;
            }
            else
            {
                // Set dashing to true
                isDashing = true;
                dash = dashTimer;
                speed = dashSpeed;
                dashQuickReset = false;

                // Set volume and pitch
                audioSource.volume = Settings.sound;
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(dashSound, 0.7f);
            }
        }

        // Release charge
        else if (charging)
        {
            // Set dashing to true
            charging = false;
            isDashing = true;
            dash = dashTimer + (chargeAmount / 2);
            speed = dashSpeed * (chargeAmount + 1 * 2);
            moveSpeed = moveSpeedHolder;
            chargeAmount = 0;
            dashQuickReset = false;

            // Set volume and pitch
            audioSource.volume = Settings.sound;
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(dashSound, 0.7f);
        }
    }

    // Rotates the players head towards the mouse
    private void RotateToMouse()
    {
        // Check if controller input allowed
        Vector2 aim = CIN._action_aim.ReadValue<Vector2>();
        if (aim.x > 0.5f || aim.y > 0.5f || aim.x < -0.5f || aim.y < -0.5f)
        {
            // Check if controller icon pressed
            if (!controllerIcon.activeSelf)
            {
                // Set last mouse pos
                lastMousePos = Mouse.current.position.ReadValue();

                // Reset cursor lock state
                Cursor.visible = false;
                controller.SetActive(true);
                controllerIcon.SetActive(true);
            }
            
            // Horizontal Movement
            float horizontal = Mathf.Clamp(aim.x, -1, 1);
            float vertical = Mathf.Clamp(aim.y, -1, 1);

            // Check horizontal and vertical
            if (horizontal != 0 || vertical != 0)
            {
                controller.transform.localPosition = new Vector2(horizontal * 35, vertical * 35);
                float angle = Mathf.Atan2(controller.transform.localPosition.y, controller.transform.localPosition.x) * Mathf.Rad2Deg;
                rotator.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
        else
        {
            // Check if controller icon pressed
            if (controllerIcon.activeSelf)
            {
                if (Mouse.current.position.ReadValue() == lastMousePos) return;
                
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

                Cursor.visible = true;
                controller.SetActive(false);
                controllerIcon.SetActive(false);
            }

            // Get mouse pos
            Vector3 mousePos = CIN._action_mouse.ReadValue<Vector2>();
            Vector3 objectPos = Camera.main.WorldToScreenPoint(rotator.position);

            mousePos.x -= objectPos.x;
            mousePos.y -= objectPos.y;

            float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            rotator.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public void OverrideDash() { shipOverridesDash = true; }
}