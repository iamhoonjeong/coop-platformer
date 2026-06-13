using UnityEngine;

public class EnemyMushroom : Enemy
{
    protected override void Update()
    {
        base.Update();
        if (isDead) return;

        HandleMovement();

        if (isGrounded) HandleTurnAround();
    }

    private void HandleTurnAround()
    {
        if (!isGroundInfrontDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleMovement()
    {
        if (idleTimer > 0) return;
        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }
}
