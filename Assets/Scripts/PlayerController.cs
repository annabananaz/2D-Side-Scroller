using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //==========================================
    //Variables
    //==========================================
    public float speed;
    public float jumpForce;
    private float moveInput;

    private bool facingRight = true;

    private Rigidbody2D rb;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatisGround;

    public int extraJumpValue;
    private int extraJumps;

    bool isTouchingFront;
    public Transform frontCheck;
    bool wallSliding;
    public float wallSlidingSpeed;

    bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    //===========================================

    void Start()
    {
        extraJumps = extraJumpValue;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);
        
        //Making sure that character sprite faces correct direction
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }

        //Horizontal Input and Movement
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, whatisGround);
        if(isTouchingFront && !isGrounded && moveInput != 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if(wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
    }

    void Update()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpValue;
        }

        if(Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if(Input.GetKeyDown(KeyCode.Space) && wallSliding)
        {
            wallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if(wallJumping)
        {
            rb.velocity = new Vector2(xWallForce * -moveInput, yWallForce);
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
}
