using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    Animator anim => GetComponent<Animator>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            anim.SetTrigger("activate");
            GameManager.instance.LevelFinished();
        }
    }
}
