using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    void Awake()
    {

    }

    void Start()
    {

    }



    void Update()
    {
        rb.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), rb.linearVelocityY);
        print(Input.GetAxisRaw("Horizontal"));
    }
}
