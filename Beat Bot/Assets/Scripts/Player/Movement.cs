using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;

    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
<<<<<<< Updated upstream:Beat Bot/Assets/Scripts/Movement.cs
    //Dash Stuff
=======
    public float maxJumpHeight = 10f;

    // Dash
>>>>>>> Stashed changes:Beat Bot/Assets/Scripts/Player/Movement.cs
    private bool isDashing;
    private float dashTimeCounter;
    private float dashDirection;
    private float dashCooldownCounter;
    private bool hasAirDashed;

    // Separate left and right movement inputs
    [SerializeField] private InputActionReference moveLeft;
    [SerializeField] private InputActionReference moveRight;

    [SerializeField] private InputActionReference jump;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private InputActionReference weaponAbility;

    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Debug.Log("PlayerMovement script is active!");
    }

    void Update()
    {
        // Read left and right movement using the new Input System
        bool leftPressed = moveLeft.action.IsPressed();
        bool rightPressed = moveRight.action.IsPressed();

        // Determine horizontal movement and animation direction
        if (leftPressed && !rightPressed)
        {
            horizontal = -1f;
        }
        else if (rightPressed && !leftPressed)
        {
            horizontal = 1f;
        }
        else if (leftPressed && rightPressed)
        {
            // If both are held, use whichever was pressed most recently
            if (moveLeft.action.WasPressedThisFrame())
            {
                horizontal = -1f;
            }
            else if (moveRight.action.WasPressedThisFrame())
            {
                horizontal = 1f;
            }
        }
        else
        {
            horizontal = 0f;
        }

        // Update the Animator's movement direction
        anim.SetFloat("MoveX", horizontal);

        // Activate the weapon ability dash
        if (weaponAbility.action.WasPressedThisFrame() && !isDashing)
        {
            if (isGrounded && dashCooldownCounter <= 0f)
            {
                StartDash();
            }
            else if (!isGrounded && !hasAirDashed)
            {
                StartDash();
            }
        }

        // Handle dash duration
        if (isDashing)
        {
            dashTimeCounter -= Time.deltaTime;

            if (dashTimeCounter <= 0)
            {
                isDashing = false;
            }
        }

        // Handle dash cooldown
        if (dashCooldownCounter > 0f)
        {
            dashCooldownCounter -= Time.deltaTime;
        }

        // Handle coyote time for jumping
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Buffer jump input
        if (jump.action.WasPressedThisFrame())
        {
            Debug.Log("JUMP PRESSED");
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump if the player is grounded or within coyote time
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
<<<<<<< Updated upstream:Beat Bot/Assets/Scripts/Movement.cs
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10);
=======
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxJumpHeight);
>>>>>>> Stashed changes:Beat Bot/Assets/Scripts/Player/Movement.cs
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // Cut the jump short when the jump button is released
        if (jump.action.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
        }


    }

    private void FixedUpdate()
    {
        // Apply dash movement or regular movement
        if (isDashing)
        {
            rb.linearVelocity = new Vector2(
                dashDirection * dashSpeed,
                rb.linearVelocity.y
            );
        }
        else
        {
            rb.linearVelocity = new Vector2(
                horizontal * speed,
                rb.linearVelocity.y
            );
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check whether the player is standing on a surface
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                hasAirDashed = false; // Reset air dash when grounded
                dashCooldownCounter = 0f; // Reset dash cooldown when grounded
                return;
            }
        }

        isGrounded = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        hasAirDashed = false; // Reset air dash when leaving the ground
        return;
    }

    private void OnEnable()
    {
        // Enable all input actions
        moveLeft.action.Enable();
        moveRight.action.Enable();
        jump.action.Enable();
        weaponAbility.action.Enable();
    }

    private void OnDisable()
    {
        // Disable all input actions
        moveLeft.action.Disable();
        moveRight.action.Disable();
        jump.action.Disable();
        weaponAbility.action.Disable();
    }

    private void StartDash()
    {
        if (horizontal != 0f)
        {
            dashDirection = Mathf.Sign(horizontal);
        }
        else
        {
            dashDirection = Mathf.Sign(transform.localScale.x);
        }

        // Mark the air dash as used if the player is airborne
        if (!isGrounded)
        {
            hasAirDashed = true;
        }

        isDashing = true;
        dashTimeCounter = dashDuration;
        dashCooldownCounter = dashCooldown;
    }
}