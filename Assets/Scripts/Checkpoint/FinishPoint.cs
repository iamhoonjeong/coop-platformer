using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    Animator anim => GetComponent<Animator>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            AudioManager.instance.PlaySFX(2);
            anim.SetTrigger("activate");
            GameManager.instance.LevelFinished();
        }
    }
}
