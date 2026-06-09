using System;
using NUnit.Framework;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement details")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] float doubleJumpForce;
    bool canDoubleJump;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    Rigidbody2D rb;
    Animator anim;

    bool isGrounded;
    bool isAirborne;
    float xInput;
    bool facingRight = true;
    int facingDir = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        HandleCollision();
        HandleInput();
        HandleMovement();
        UpdateAirborneStatus();
        HandleFlip();
        HandleAnimations();
    }

    void UpdateAirborneStatus()
    {
        if (isGrounded && isAirborne)
        {
            HandleLanding();
        }

        if (!isGrounded && !isAirborne)
        {
            BecomeAirborne();
        }
    }

    void BecomeAirborne()
    {
        isAirborne = true;
    }

    void HandleLanding()
    {
        isAirborne = false;
        canDoubleJump = true;
    }

    void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
        }
    }

    void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
    }

    void Jump() => rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
    void DoubleJump()
    {
        canDoubleJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJumpForce);
    }

    void HandleMovement() => rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);

    void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetBool("isGrounded", isGrounded);
    }

    void HandleCollision() => isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);

    void HandleFlip()
    {
        if (rb.linearVelocityX < 0 && facingRight || rb.linearVelocityX > 0 && !facingRight) Flip();
    }

    void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    void OnDrawGizmos() => Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
}
