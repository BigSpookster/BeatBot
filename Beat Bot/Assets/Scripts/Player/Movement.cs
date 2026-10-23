
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;

    // Direction
    private bool facingRight = false;
    public bool FacingRight => facingRight;

    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    public float maxJumpHeight = 10f;

    // Dash
    private bool isDashing;
    private float dashTimeCounter;
    private float dashDirection;
    private float dashCooldownCounter;
    private bool hasAirDashed;

    [SerializeField] private InputActionReference moveLeft;
    [SerializeField] private InputActionReference moveRight;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private InputActionReference weaponAbility;

    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        bool leftPressed = moveLeft.action.IsPressed();
        bool rightPressed = moveRight.action.IsPressed();

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
            if (moveLeft.action.WasPressedThisFrame())
                horizontal = -1f;
            else if (moveRight.action.WasPressedThisFrame())
                horizontal = 1f;
        }
        else
        {
            horizontal = 0f;
        }

        // Remember the last direction pressed.
        if (horizontal < 0f)
            facingRight = false;
        else if (horizontal > 0f)
            facingRight = true;

        if (anim != null)
            anim.SetFloat("MoveX", horizontal);

        // Weapon ability dash
        if (weaponAbility.action.WasPressedThisFrame() && !isDashing)
        {
            if (isGrounded && dashCooldownCounter <= 0f)
                StartDash();
            else if (!isGrounded && !hasAirDashed)
                StartDash();
        }

        if (isDashing)
        {
            dashTimeCounter -= Time.deltaTime;

            if (dashTimeCounter <= 0f)
                isDashing = false;
        }

        if (dashCooldownCounter > 0f)
            dashCooldownCounter -= Time.deltaTime;

        // Coyote time
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
        if (jump.action.WasPressedThisFrame())
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxJumpHeight);
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }

        // Variable jump height
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
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                hasAirDashed = false;
                dashCooldownCounter = 0f;
                return;
            }
        }

        isGrounded = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        hasAirDashed = false;
    }

    private void OnEnable()
    {
        moveLeft.action.Enable();
        moveRight.action.Enable();
        jump.action.Enable();
        weaponAbility.action.Enable();
    }

    private void OnDisable()
    {
        moveLeft.action.Disable();
        moveRight.action.Disable();
        jump.action.Disable();
        weaponAbility.action.Disable();
    }

    private void StartDash()
    {
        if (horizontal != 0f)
            dashDirection = Mathf.Sign(horizontal);
        else
            dashDirection = facingRight ? 1f : -1f;

        if (!isGrounded)
            hasAirDashed = true;

        isDashing = true;
        dashTimeCounter = dashDuration;
        dashCooldownCounter = dashCooldown;
    }

    // Attack 3 lunge in the direction selected by PlayerCombat.
    public void LungeForward(bool moveRight)
    {
        float direction = moveRight ? 1f : -1f;
        rb.MovePosition(rb.position + Vector2.right * direction * 1.5f);
    }
}
