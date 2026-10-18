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
    //Change these values to adjust the coyote and jump buffer time.
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    //Use these variables for the dash mechanics.
    private bool isDashing;
    private float dashTimeCounter;
    private float dashDirection;
    private float dashCooldownCounter;

    [SerializeField] private InputActionReference horizontalMovement;
    
    [SerializeField] private InputActionReference jump;

    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [SerializeField] private InputActionReference weaponAbility;

    //These are the variables for the dash.
    //Used for BB speed in the dash
    [SerializeField] private float dashSpeed = 14f;
    //Used for how long the dash is active for
    [SerializeField] private float dashDuration = 0.18f;
    //Used for how long dash cant be used again for.
    [SerializeField] private float dashCooldown = 0.6f;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Debug.Log("PlayerMovement script is active!");
    }

    void Update()
    {
        horizontal = horizontalMovement.action.ReadValue<Vector2>().x;
        if(weaponAbility.action.WasPressedThisFrame() && !isDashing && dashCooldownCounter <= 0 ){
            StartDash();
        }
        if(isDashing){
            dashTimeCounter -= Time.deltaTime;
            if(dashTimeCounter <= 0){
                isDashing = false;
            }
        }
        if(dashCooldownCounter > 0f){
            dashCooldownCounter -= Time.deltaTime;
        }

        // Coyote Time Code
        //This runs if bb is touching a floor.
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            //This runs if bb is not touching the floor.
            //It  counts down to 0 and the player has time to jump during that window.        
            coyoteTimeCounter -= Time.deltaTime;

        // Jump Buffer
        //This checks if the jump button is pressed.
        //If it is the jump buffer window is updated and saves the input for some time.
        if (jump.action.WasPressedThisFrame())
            jumpBufferCounter = jumpBufferTime;
            //if jump is not pressed the window counts down back to 0.
        else
            jumpBufferCounter -= Time.deltaTime;

        // Jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 10);

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
        //This is used to make jump more dynamic with the button held length.
        if (jump.action.WasReleasedThisFrame() && rb.linearVelocity.y > 0f){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        // This if statement is used to keep bb facing the correct direction while moving.
        //It runs if there is a horizontal input.
        if (horizontal != 0)
            //This changes the size and orientation of bb based on his x,y, and z values.
            transform.localScale = new Vector3(Mathf.Sign(horizontal) * Mathf.Abs(transform.localScale.x),transform.localScale.y,1);
    }

    private void FixedUpdate()
    {
        if(isDashing){
            rb.linearVelocity = new Vector2(dashDirection * dashSpeed, rb.linearVelocity.y); 
        }
        else{
            // This line is used to control BeatBots speed.
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
        }
    }

  
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }



    private void OnEnable()
    {
        horizontalMovement.action.Enable();
        jump.action.Enable();
        weaponAbility.action.Enable();

    }

    private void OnDisable()
    {
        horizontalMovement.action.Disable();
        jump.action.Disable();
        weaponAbility.action.Disable();
    }

    private void StartDash(){
        //This checks if BB is pressing left or right input
        if(horizontal != 0f){
            //This converts the input based on the direction inputted
            dashDirection = Mathf.Sign(horizontal);
        }
        else{
            dashDirection = Mathf.Sign(transform.localScale.x);
        }
        //This tells the system that the dash ability is active.
        isDashing = true;
        //This updates the activation timer to the length of the ability.
        dashTimeCounter = dashDuration;
        dashCooldownCounter = dashCooldown;
    }

}