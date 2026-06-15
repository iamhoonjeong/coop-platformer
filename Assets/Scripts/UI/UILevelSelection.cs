using UnityEngine;
using UnityEngine.SceneManagement;

public class UILevelSelection : MonoBehaviour
{
    [SerializeField] UILevelButton buttonPrefab;
    [SerializeField] Transform buttonsParent;

    [SerializeField] bool[] levelsUnlocked;

    void Start()
    {
        LoadLevelsInfo();
        CreateLevelButtons();
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
