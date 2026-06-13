using UnityEngine;

public class EnemyTrunk : Enemy
{
    [Header("Trunk details")]
    [SerializeField] EnemyBullet bulletPrefab;
    [SerializeField] Transform gunPoint;
    [SerializeField] float bulletSpeed = 7f;
    [SerializeField] float attackCooldown = 1.5f;
    float lastTimeAttacked;


    protected override void Update()
    {
        base.Update();
        if (isDead) return;


        bool canAttack = Time.time > lastTimeAttacked + attackCooldown;

        if (isPlayerDetected && canAttack) Attack();

        HandleMovement();

        if (isGrounded) HandleTurnAround();
    }


    void Attack()
    {
        idleTimer = idleDuration + attackCooldown;
        lastTimeAttacked = Time.time;
        anim.SetTrigger("attack");
    }

    void CreateBullet()
    {
        EnemyBullet newBullet = Instantiate(bulletPrefab, gunPoint.position, Quaternion.identity);
        Vector2 bulletVelocity = new Vector2(facingDir * bulletSpeed, 0);
        newBullet.SetVelocity(bulletVelocity);

        if (facingDir == 1)
        {
            newBullet.FlipSprite();
        }

        Destroy(newBullet.gameObject, 10);
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
