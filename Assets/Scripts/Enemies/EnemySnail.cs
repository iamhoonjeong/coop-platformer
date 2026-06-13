using UnityEngine;

public class EnemySnail : Enemy
{
    [Header("Snail details")]
    [SerializeField] EnemySnailBody bodyPrefab;
    [SerializeField] float maxSpeed = 10f;
    bool hasBody = true;

    protected override void Update()
    {
        base.Update();
        if (isDead) return;

        HandleMovement();

        if (isGrounded) HandleTurnAround();
    }

    public override void Die()
    {
        if (hasBody)
        {
            canMove = false;
            hasBody = false;
            anim.SetTrigger("hit");

            rb.linearVelocity = Vector2.zero;
            idleDuration = 0;
        }
        else if (!canMove)
        {
            anim.SetTrigger("hit");
            canMove = true;
            moveSpeed = maxSpeed;
        }
        else
        {
            base.Die();
        }
    }

    private void HandleTurnAround()
    {
        bool canFlipFromLedge = !isGroundInfrontDetected && hasBody;
        if (canFlipFromLedge || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleMovement()
    {
        if (idleTimer > 0) return;

        if (!canMove) return;

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);
    }

    void CreateBody()
    {
        EnemySnailBody newBody = Instantiate(bodyPrefab, transform.position, Quaternion.identity);


        if (Random.Range(0, 100) < 50)
        {
            deathRotationDirection *= -1;
            newBody.SetupBody(deathImpactSpeed, deathRotationSpeed * deathRotationDirection, facingDir);
        }

        Destroy(newBody.gameObject, 10);
    }
    protected override void Flip()
    {
        base.Flip();

        if (!hasBody)
        {
            anim.SetTrigger("wallHit");
        }
    }
}
