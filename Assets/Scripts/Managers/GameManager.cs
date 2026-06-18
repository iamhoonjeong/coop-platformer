using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    UIInGame inGameUI;

    [Header("Level Management")]
    [SerializeField] float levelTimer;
    [SerializeField] int currentLevelIndex;
    int nextLevelIndex;

    [Header("Fruits Management")]
    public bool fruitsAreRandom;
    public int fruitsCollected;
    public int totalFruits;
    public Transform fruitParent;

    [Header("Checkpoints")]
    public bool canReactivate;

    [Header("Managers")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] PlayerManager playerManager;
    [SerializeField] SkinManager skinManager;
    [SerializeField] DifficultyManager difficultyManager;
    [SerializeField] ObjectCreator objectCreator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);

        }
    }

    void Start()
    {
        inGameUI = UIInGame.instance;

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;


        nextLevelIndex = currentLevelIndex + 1;
        CollectFruitsInfo();
        CreateManagersIfNeeded();
    }

    void Update()
    {
        levelTimer += Time.deltaTime;

        inGameUI.UpdateTimerUI(levelTimer);
    }

    void CreateManagersIfNeeded()
    {
        if (AudioManager.instance == null) Instantiate(audioManager);
        if (PlayerManager.instance == null) Instantiate(playerManager);
        if (SkinManager.instance == null) Instantiate(skinManager);
        if (DifficultyManager.instance == null) Instantiate(difficultyManager);
        if (ObjectCreator.instance == null) Instantiate(objectCreator);
    }

    private void CollectFruitsInfo()
    {
        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruits = allFruits.Length;

        inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);

        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
    }


    [ContextMenu("Parent All Fruits")]
    private void ParentAllTheFruits()
    {
        if (fruitParent == null) return;

        Fruit[] allFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None);

        foreach (Fruit fruit in allFruits)
        {
            fruit.transform.parent = fruitParent;
        }
    }



    public void AddFruit()
    {
        fruitsCollected++;
        inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public void RemoveFruit()
    {
        fruitsCollected--;
        inGameUI.UpdateFruitUI(fruitsCollected, totalFruits);
    }

    public int FruitsCollected() => fruitsCollected;

    public bool FruitsHaveRandomLoom()
    {
        return fruitsAreRandom;
    }

    public void LevelFinished()
    {
        SaveLevelProgression();
        SaveBestTime();
        SaveFruitsInfo();
        LoadNextScene();
    }

    void SaveFruitsInfo()
    {
        int fruitsCollectedBefore = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruisCollected", fruitsCollected);

        if (fruitsCollectedBefore < fruitsCollected) PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruisCollected", fruitsCollected);

        int totalFruitsInBank = PlayerPrefs.GetInt("TotalFruitsAmount");

        PlayerPrefs.SetInt("TotalFruitsAmount", totalFruitsInBank + fruitsCollected);
    }

    void SaveBestTime()
    {
        float lastTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestTime", 99);

        if (levelTimer < lastTime)
            PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestTime", levelTimer);
    }

    private void SaveLevelProgression()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);

        if (!NoMoreLevels())
        {
            PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);

            SkinManager skinManager = SkinManager.instance;

            if (skinManager) PlayerPrefs.SetInt("LastUsedSkin", SkinManager.instance.GetSkinId());
        }

    }

    public void RestartLevel()
    {
        UIInGame.instance.fadeEffect.ScreenFade(1, 0.75f, LoadCurrentScene);
    }

    private void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);

    void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");

    void LoadNextLevel()
    {
        SceneManager.LoadScene("Level_" + nextLevelIndex);
    }

    private void LoadNextScene()
    {
        UIFadeEffect fadeEffect = UIInGame.instance.fadeEffect;


        if (NoMoreLevels()) UIInGame.instance.fadeEffect.ScreenFade(1, 1.5f, LoadTheEndScene);
        else fadeEffect.ScreenFade(1, 1.5f, LoadNextLevel);
    }

    bool NoMoreLevels()
    {
        int lastlevelIndex = SceneManager.sceneCountInBuildSettings - 2;
        bool noMoreLevels = currentLevelIndex == lastlevelIndex;
        return noMoreLevels;
    }
}
