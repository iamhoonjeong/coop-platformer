using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator anim => GetComponent<Animator>();
    bool active;

    [SerializeField] bool canBeReactivated;

    void Start()
    {
        canBeReactivated = GameManager.instance.canReactivate;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && !canBeReactivated) return;
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        active = true;
        anim.SetTrigger("activate");
        GameManager.instance.UpdateRespawnPosition(transform);
    }
}
