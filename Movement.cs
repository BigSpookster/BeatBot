using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    //Change these values to adjust the coyote and jump buffer time.
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Debug.Log("PlayerMovement script is active!");
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        // This line is used to control BeatBots speed.
        rb.linearVelocity = new Vector2(horizontal * 5, rb.linearVelocity.y);

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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
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

        // This if statement is used to keep bb facing the correct direction while moving.
        //It runs if there is a horizontal input.
        if (horizontal != 0)
            //This changes the size and orientation of bb based on his x,y, and z values.
            transform.localScale = new Vector3(Mathf.Sign(horizontal) * Mathf.Abs(transform.localScale.x),transform.localScale.y,1);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}