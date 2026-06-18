using UnityEngine;
using UnityEngine.EventSystems;

public class UIDifficulty : MonoBehaviour
{
    UIMainMenu mainMenu;
    [SerializeField] GameObject firstSelected;
    DifficultyManager difficultyManager;

    void Awake()
    {
        mainMenu = GetComponentInParent<UIMainMenu>();
    }
    void OnEnable()
    {
        mainMenu.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void Start()
    {
        difficultyManager = DifficultyManager.instance;
    }

    public void SetEasyMode() => difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetNormalMode() => difficultyManager.SetDifficulty(DifficultyType.Normal);
    public void SetHardMode() => difficultyManager.SetDifficulty(DifficultyType.Hard);
}
