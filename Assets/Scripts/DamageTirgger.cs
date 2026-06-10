using UnityEngine;

public class DamageTirgger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        player?.Knockback();
    }
}
