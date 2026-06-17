using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    UIFadeEffect fadeEffect;
    public string FirstLevelName;
    [SerializeField] GameObject[] uiElements;
    [SerializeField] GameObject continueButton;

    [Header("Interactive Camera")]
    [SerializeField] MenuCharacter menuCharacter;
    [SerializeField] CinemachineCamera cinemachine;
    [SerializeField] Transform mainMenuPoint;
    [SerializeField] Transform skinSelectionPoint;

    void Awake()
    {
        fadeEffect = GetComponentInChildren<UIFadeEffect>();
    }

    void Start()
    {
        if (HasLevelProgression()) continueButton.SetActive(true);
        fadeEffect.ScreenFade(0, 1.5f);
    }

    public void SwitchUI(GameObject uiToEnable)
    {
        foreach (GameObject ui in uiElements)
        {
            ui.SetActive(false);
        }

        AudioManager.instance.PlaySFX(4);

        uiToEnable.SetActive(true);
    }

    public void NewGame()
    {
        fadeEffect.ScreenFade(1, 1.5f, LoadLevelScene);
        AudioManager.instance.PlaySFX(4);
    }

    void LoadLevelScene()
    {
        SceneManager.LoadScene(FirstLevelName);
    }

    bool HasLevelProgression()
    {
        bool hasLevelProgression = PlayerPrefs.GetInt("ContinueLevelNumber", 0) > 0;
        return hasLevelProgression;
    }

    public void ContinueGame()
    {
        int difficultyIndex = PlayerPrefs.GetInt("GameDifficulty", 1);
        int levelToLoad = PlayerPrefs.GetInt("ContinueLevelNumber", 0);
        int lastSavedSkin = PlayerPrefs.GetInt("LastUsedSkin");

        SkinManager.instance.SetSkinId(lastSavedSkin);

        DifficultyManager.instance.LoadDifficulty(difficultyIndex);

        SceneManager.LoadScene("Level_" + levelToLoad);
        AudioManager.instance.PlaySFX(4);
    }

    public void MoveCameraToMainMenu()
    {
        menuCharacter.MoveTo(mainMenuPoint);
        cinemachine.Follow = mainMenuPoint;
    }

    public void MoveCameraToSkinMenu()
    {
        menuCharacter.MoveTo(skinSelectionPoint);
        cinemachine.Follow = skinSelectionPoint;
    }
}
