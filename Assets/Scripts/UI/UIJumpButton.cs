using UnityEngine;
using UnityEngine.EventSystems;

public class UIJumpButton : MonoBehaviour, IPointerDownHandler
{
    Player player;

    public void UpdatePlayersRef(Player newPlayer)
    {
        player = newPlayer;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        player.JumpButton();
    }
}
