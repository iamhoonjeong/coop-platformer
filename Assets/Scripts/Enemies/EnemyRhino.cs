using Unity.Cinemachine;
using UnityEngine;

public class EnemyRhino : Enemy
{
    [Header("Rhino details")]
    [SerializeField] float maxSpeed;
    [SerializeField] float speedUpRate = 0.6f;
    [SerializeField] float defaultSpeed;
    [SerializeField] Vector2 impactPower;

    [Header("Effects")]
    [SerializeField] ParticleSystem dustFx;
    [SerializeField] Vector2 cameraImpulseDir;
    CinemachineImpulseSource impulseSource;

    protected override void Start()
    {
        base.Start();

        canMove = false;
        defaultSpeed = moveSpeed;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    protected override void Update()
    {
        base.Update();
        HandleCharge();
    }

    void HitWallImpact()
    {
        dustFx.Play();
        impulseSource.DefaultVelocity = new Vector2(cameraImpulseDir.x * facingDir, cameraImpulseDir.y);
        impulseSource.GenerateImpulse();
    }

    void HandleCharge()
    {
        if (!canMove) return;
        HandleSpeedUp();

        rb.linearVelocity = new Vector2(moveSpeed * facingDir, rb.linearVelocityY);

        if (isWallDetected) WallHit();

        if (!isGroundInfrontDetected) TurnAround();


    }

    private void HandleSpeedUp()
    {
        moveSpeed = moveSpeed + Time.deltaTime * speedUpRate;
        if (moveSpeed >= maxSpeed) maxSpeed = moveSpeed;
    }

    private void TurnAround()
    {
        SpeedReset();
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        Flip();
    }

    void WallHit()
    {
        HitWallImpact();
        canMove = false;
        SpeedReset();
        anim.SetBool("hitWall", true);
        rb.linearVelocity = new Vector2(impactPower.x * -facingDir, impactPower.y);
    }

    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
    }

    void ChargeIsOver()
    {
        anim.SetBool("hitWall", false);
        Invoke(nameof(Flip), 1);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        // playerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, playerDetectionDistance, whatIsPlayer);

        if (isPlayerDetected && isGrounded) canMove = true;
    }
}
