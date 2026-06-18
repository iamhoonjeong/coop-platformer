using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UILevelSelection : MonoBehaviour
{
    UIMainMenu mainMenuUI;
    [SerializeField] GameObject firstSelected;
    [SerializeField] UILevelButton buttonPrefab;
    [SerializeField] Transform buttonsParent;

    [SerializeField] bool[] levelsUnlocked;


    void Awake()
    {
        mainMenuUI = GetComponentInParent<UIMainMenu>();

        LoadLevelsInfo();
        CreateLevelButtons();
    }

    void OnEnable()
    {
        mainMenuUI.UpdateLastSelected(firstSelected);
        GameObject firstLevelButtom = buttonsParent.GetChild(0).gameObject;

        if (firstLevelButtom != null)
        {
            EventSystem.current.SetSelectedGameObject(firstLevelButtom);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    void CreateLevelButtons()
    {
        int levelsAmount = SceneManager.sceneCountInBuildSettings - 1;

        for (int i = 1; i < levelsAmount; i++)
        {
            if (!isLevelUnlocked(i)) return;

            UILevelButton newButton = Instantiate(buttonPrefab, buttonsParent);
            newButton.SetupButton(i);
        }
    }

    bool isLevelUnlocked(int levelIndex) => levelsUnlocked[levelIndex];

    void LoadLevelsInfo()
    {
        int levelsAmount = SceneManager.sceneCountInBuildSettings - 1;

        levelsUnlocked = new bool[levelsAmount];

        for (int i = 1; i < levelsAmount; i++)
        {
            bool levelUnlocked = PlayerPrefs.GetInt("Level" + i + "Unlocked") == 1;
            if (levelUnlocked)
            {
                levelsUnlocked[i] = true;
            }
        }

        levelsUnlocked[1] = true;
    }
}
