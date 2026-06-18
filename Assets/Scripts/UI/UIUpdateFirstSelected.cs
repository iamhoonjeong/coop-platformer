using UnityEngine;
using UnityEngine.EventSystems;

public class UIUpdateFirstSelected : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;

    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
