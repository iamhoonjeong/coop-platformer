using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIInGame : MonoBehaviour
{
    public static UIInGame instance;
    public UIFadeEffect fadeEffect { get; private set; }
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI fruitText;
    [SerializeField] GameObject pauseUI;

    bool isPaused;

    void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UIFadeEffect>();
    }

    void Start()
    {
        fadeEffect.ScreenFade(0, 1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseButton();
        }
    }

    public void PauseButton()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1;
            pauseUI.SetActive(false);
        }
        else
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }
    }

    public void GoToMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateFruitUI(int collecteFruits, int totalFruits)
    {
        fruitText.text = collecteFruits + "/" + totalFruits;
    }

    public void UpdateTimerUI(float timer)
    {
        timerText.text = timer.ToString("00") + " s";
    }
}
