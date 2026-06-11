using UnityEngine;

public class Trap_Trampoline : MonoBehaviour
{
    protected Animator anim;
    [SerializeField] float pushPower;
    [SerializeField] float duration = 0.5f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            player.Push(transform.up * pushPower, duration);
            anim.SetTrigger("activate");
        }
    }
}
