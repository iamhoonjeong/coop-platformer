using UnityEngine;

public class EnemySnailBody : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;
    float zRotation;

    public void SetupBody(float yVelocity, float zRotation, int facingDir)
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, yVelocity);

        this.zRotation = zRotation;

        if (facingDir == 1)
        {
            sr.flipX = true;
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, zRotation * Time.deltaTime);
    }
}
