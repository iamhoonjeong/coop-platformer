using UnityEngine;

public class EnemyMushroom : Enemy
{
    BoxCollider2D cd;

    protected override void Awake()
    {
        base.Awake();

        cd = GetComponent<BoxCollider2D>();
    }

    protected override void Update()
    {
        base.Update();

        anim.SetFloat("xVelocity", rb.linearVelocityX);

        if (isDead) return;

        HandleMovement();
        HandleCollision();

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
        if (isGroundInfrontDetected) rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }

    public override void Die()
    {
        base.Die();

        cd.enabled = false;
    }
}
