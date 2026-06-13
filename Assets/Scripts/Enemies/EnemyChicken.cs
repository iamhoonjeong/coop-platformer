using UnityEngine;

public class EnemyChicken : Enemy
{
    [Header("Chicken details")]
    [SerializeField] float aggroDuration;
    float aggroTimer;
    [SerializeField] float detectionRange;
    bool canFlip = true;

    protected override void Update()
    {
        base.Update();

        aggroTimer -= Time.deltaTime;

        if (isDead) return;

        if (isPlayerDetected)
        {
            canMove = true;
            aggroTimer = aggroDuration;
        }

        if (aggroTimer < 0) canMove = false;

        HandleMovement();

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
}
