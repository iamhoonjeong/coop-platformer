using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    Rigidbody2D rb;
    Animator anim;

    float xInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        HandleAnimations();
        HandleMovement();
    }

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);
    }

    void HandleAnimations()
    {
        anim.SetBool("isRunning", rb.linearVelocityX != 0);
    }
}
