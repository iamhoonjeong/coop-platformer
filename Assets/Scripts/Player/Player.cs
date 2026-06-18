using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject fruitDrop;
    [SerializeField] DifficultyType gameDifficulty;
    GameManager gameManager;

    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D cd;
    bool canBeControlled = false;

    public PlayerInput playerInput { get; private set; }
    Vector2 moveInput;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] float doubleJumpForce;
    float defaultGravityScale;
    bool canDoubleJump;

    [Header("Buffer & Coyote jump")]
    [SerializeField] float bufferJumpWindow = 0.25f;
    float bufferJumpActivated = -1;
    [SerializeField] float coyoteJumpWindow = 0.5f;
    float coyoteJumpActicated = -1;

    [Header("Wall interactions")]
    [SerializeField] float wallJumpDuration = 0.6f;
    [SerializeField] Vector2 wallJumpForce;
    bool isWallJumping;

    [Header("Knockback")]
    [SerializeField] float knockbackDuration = 1f;
    [SerializeField] Vector2 knockbackPower;
    bool isKnocked;

    [Header("Collision")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [Space]
    [SerializeField] Transform enemyCheck;
    [SerializeField] float enemyCheckRadius;
    [SerializeField] LayerMask whatIsEnemy;

    [Header("Player Visuals")]
    [SerializeField] AnimatorOverrideController[] animators;
    [SerializeField] GameObject deathVFX;
    [SerializeField] ParticleSystem dustFx;
    [SerializeField] int skinId;

    bool isGrounded;
    bool isAirborne;
    bool isWallDetected;

    bool facingRight = true;
    int facingDir = 1;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();

        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Jump.performed += ctx => JumpButton();
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Jump.performed -= ctx => JumpButton();
        playerInput.Player.Movement.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled -= ctx => moveInput = Vector2.zero;
    }

    void Start()
    {
        defaultGravityScale = rb.gravityScale;
        gameManager = GameManager.instance;

        UpdateGameDifficulty();
        RespawnFinished(false);
        UpdateSkin();
    }

    void Update()
    {
        UpdateAirborneStatus();

        if (!canBeControlled)
        {
            HandleCollision();
            HandleAnimations();
            return;
        }

        if (isKnocked) return;

        HandleEnemyDetection();
        // HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleCollision();
        HandleAnimations();
    }

    public void Damage()
    {
        if (gameDifficulty == DifficultyType.Normal)
        {
            if (gameManager.FruitsCollected() <= 0)
            {
                Die();
                gameManager.RestartLevel();
            }
            else
            {
                ObjectCreator.instance.CreateObject(fruitDrop, transform, true);
                gameManager.RemoveFruit();
            }
            return;
        }

        if (gameDifficulty == DifficultyType.Hard)
        {
            Die();
            gameManager.RestartLevel();
        }
    }

    private void UpdateGameDifficulty()
    {
        DifficultyManager difficultyManager = DifficultyManager.instance;

        if (difficultyManager) gameDifficulty = difficultyManager.difficulty;
    }

    public void UpdateSkin()
    {
        SkinManager skinManager = SkinManager.instance;

        if (skinManager == null) return;

        anim.runtimeAnimatorController = animators[skinManager.choosenSkinId];
    }

    void HandleEnemyDetection()
    {
        if (rb.linearVelocityY >= 0) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius, whatIsEnemy);

        foreach (var enemy in colliders)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();

            if (newEnemy)
            {
                AudioManager.instance.PlaySFX(1);
                newEnemy.Die();
                Jump();
            }
        }
    }

    public void RespawnFinished(bool finished)
    {
        float gravityScsle = defaultGravityScale;

        if (finished)
        {
            rb.gravityScale = gravityScsle;
            canBeControlled = true;
            cd.enabled = true;
            AudioManager.instance.PlaySFX(11);
        }
        else
        {
            rb.gravityScale = 0;
            canBeControlled = false;
            cd.enabled = false;
        }
    }

    public void Knockback(float sourceDamageXPosition)
    {
        float knockbackDir = 1f;

        if (transform.position.x < sourceDamageXPosition)
        {
            knockbackDir = -1;
        }

        if (isKnocked) return;

        AudioManager.instance.PlaySFX(9);
        CameraManager.instance.ScreenShake(knockbackDir);
        StartCoroutine(KnockbackRoutine());

        rb.linearVelocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
    }

    IEnumerator KnockbackRoutine()
    {
        isKnocked = true;
        anim.SetBool("isKnocked", isKnocked);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
        anim.SetBool("isKnocked", isKnocked);
    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(0);

        GameObject newDeathVfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Push(Vector2 direction, float duration = 0)
    {
        StartCoroutine(PushCoroutine(direction, duration));
    }

    IEnumerator PushCoroutine(Vector2 direction, float duration)
    {
        canBeControlled = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        canBeControlled = true;
    }

    void UpdateAirborneStatus()
    {
        if (isGrounded && isAirborne) HandleLanding();
        if (!isGrounded && !isAirborne) BecomeAirborne();
    }

    void BecomeAirborne()
    {
        isAirborne = true;

        if (rb.linearVelocity.y < 0)
        {
            ActivateCoyoteJump();
        }
    }

    void HandleLanding()
    {
        dustFx.Play();

        isAirborne = false;
        canDoubleJump = true;

        AttemptBufferJump();
    }

    void HandleInput()
    {
        // xInput = Input.GetAxisRaw("Horizontal");
        // yInput = Input.GetAxisRaw("Vertical");
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     JumpButton();
        //     RequestBufferJump();
        // }
    }

    #region Buffer & Coyote Jump
    void RequestBufferJump()
    {
        if (isAirborne)
        {
            bufferJumpActivated = Time.time;
        }
    }

    void AttemptBufferJump()
    {
        if (Time.time < bufferJumpActivated + bufferJumpWindow)
        {
            bufferJumpActivated = Time.time - 1;
            Jump();
        }
    }

    void ActivateCoyoteJump()
    {
        coyoteJumpActicated = Time.time;
    }

    void CancelCoyoteJump()
    {
        coyoteJumpActicated = Time.time - 1;
    }
    #endregion

    void JumpButton()
    {
        bool coyoteJumpAvailable = Time.time < coyoteJumpActicated + coyoteJumpWindow;

        if (isGrounded || coyoteJumpAvailable)
        {
            Jump();
        }
        else if (isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }

        CancelCoyoteJump();
    }

    void Jump()
    {
        dustFx.Play();
        AudioManager.instance.PlaySFX(3);
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
    }

    void DoubleJump()
    {
        dustFx.Play();
        AudioManager.instance.PlaySFX(3);
        isWallJumping = false;
        canDoubleJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJumpForce);
    }

    void WallJump()
    {
        dustFx.Play();
        AudioManager.instance.PlaySFX(12);
        canDoubleJump = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * -facingDir, wallJumpForce.y);
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }

    IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;

        yield return new WaitForSeconds(wallJumpDuration);

        isWallJumping = false;
    }

    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocityY < 0;
        float yModifier = moveInput.y < 0 ? 1f : 0.05f;

        if (!canWallSlide) return;

        rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * yModifier);
    }

    void HandleMovement()
    {
        if (isWallDetected) return;
        if (isWallJumping) return;
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocityY);
    }

    void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    }

    void HandleFlip()
    {
        if (moveInput.x < 0 && facingRight || moveInput.x > 0 && !facingRight) Flip();
    }

    void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDir), transform.position.y));
    }
}
