using System.Collections;
using UnityEngine;

public class TrapFallingPlatform : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    BoxCollider2D[] colliders;

    [SerializeField] float speed = 0.75f;
    [SerializeField] float travelDistance;
    Vector3[] wayPoints;
    int wayPointIndex;
    bool canMove = false;

    [Header("Platform fall details")]
    [SerializeField] float impactSpeed = 3f;
    [SerializeField] float impactDuration = 0.1f;
    float impactTimer;
    bool impactHappend;
    [Space]
    [SerializeField] float fallDelay = 0.5f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<BoxCollider2D>();
    }

    IEnumerator Start()
    {
        SetupWaypoints();
        float randomDelay = Random.Range(0, 0.6f);

        yield return new WaitForSeconds(randomDelay);

        canMove = true;

    }

    void ActivatePlatform()
    {
        canMove = true;
    }

    void Update()
    {
        HandleImpact();
        HandleMovement();
    }

    void SetupWaypoints()
    {
        wayPoints = new Vector3[2];

        float yOffset = travelDistance / 2;

        wayPoints[0] = transform.position + new Vector3(0, yOffset, 0);
        wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0);
    }

    void HandleMovement()
    {
        if (!canMove) return;

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex], speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, wayPoints[wayPointIndex]) < 0.1f)
        {
            wayPointIndex++;

            if (wayPointIndex >= wayPoints.Length)
            {
                wayPointIndex = 0;
            }
        }
    }

    void HandleImpact()
    {
        if (impactTimer < 0) return;

        impactTimer -= Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3.down * 10), impactSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (impactHappend) return;

        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            Invoke(nameof(SwitchOffPlatform), fallDelay);
            impactTimer = impactDuration;
            impactHappend = true;
        }
    }

    void SwitchOffPlatform()
    {
        anim.SetTrigger("deactivate");

        canMove = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3.5f;
        rb.linearDamping = 0.5f;

        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
