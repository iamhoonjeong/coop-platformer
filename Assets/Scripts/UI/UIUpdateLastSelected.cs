using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIUpdateLastSelected : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    UIMainMenu mainMenu;

    void Awake()
    {
        mainMenu = GetComponentInParent<UIMainMenu>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        mainMenu.UpdateLastSelected(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
