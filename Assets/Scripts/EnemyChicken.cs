using UnityEngine;

public class EnemyChicken : Enemy
{
    [Header("Chicken details")]
    [SerializeField] float aggroDuration;
    float aggroTimer;
    [SerializeField] bool playerDetected;
    [SerializeField] float detectionRange;
    bool canFlip = true;

    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.linearVelocityX);
        aggroTimer -= Time.deltaTime;

        if (isDead) return;

        if (playerDetected)
        {
            canMove = true;
            aggroTimer = aggroDuration;
        }

        if (aggroTimer < 0) canMove = false;

        HandleMovement();
        HandleCollision();

        if (isGrounded) HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected)
        {
            Flip();
            canMove = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleMovement()
    {
        if (!canMove) return;

        HandleFlip(player.position.x);

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }

    protected override void HandleFlip(float xValue)
    {
        if (xValue < transform.position.x && facingRight || xValue > transform.position.x && !facingRight)
        {
            if (canFlip)
            {
                canFlip = false;
                Invoke(nameof(Flip), 0.3f);
            }
        }
    }

    protected override void Flip()
    {
        base.Flip();
        canFlip = true;
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, detectionRange, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (detectionRange * facingDir), transform.position.y));
    }
}
