using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void FinishRespown()
    {
        player.RespawnFinished(true);
    }
}
