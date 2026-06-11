using UnityEngine;

public class TrapFireButton : MonoBehaviour
{
    Animator anim;
    TrapFire trapfire;

    void Awake()
    {
        anim = GetComponent<Animator>();
        trapfire = GetComponentInParent<TrapFire>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player)
        {
            anim.SetTrigger("activate");
            trapfire.SwitchOffFire();
        }
    }
}
