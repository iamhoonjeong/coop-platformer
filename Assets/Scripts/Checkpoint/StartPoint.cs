using UnityEngine;

public class StartPoint : MonoBehaviour
{
    Animator anim => GetComponent<Animator>();

    void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            anim.SetTrigger("activate");
        }
    }
}
