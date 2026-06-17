using UnityEngine;

public class LevelCameraTirigger : MonoBehaviour
{
    LevelCamera levelCamera;

    void Awake()
    {
        levelCamera = GetComponentInParent<LevelCamera>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            levelCamera.EnableCamera(true);
            levelCamera.SetNewTarget(player.transform);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
        {
            levelCamera.EnableCamera(false);
        }
    }
}
