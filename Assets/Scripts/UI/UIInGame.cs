using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIInGame : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;

    PlayerInput playerInput;
    Player player;
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
        playerInput = new PlayerInput();
    }
    void OnEnable()
    {
        playerInput.Enable();
        playerInput.UI.Pause.performed += ctx => PauseButton();
        playerInput.UI.Navigate.performed += ctx => UpdateSelected();
    }

    void OnDisable()
    {
        playerInput.Disable();
        playerInput.UI.Pause.performed -= ctx => PauseButton();
        playerInput.UI.Navigate.performed -= ctx => UpdateSelected();
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


    void UpdateSelected()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }

    public void PauseButton()
    {
        player = PlayerManager.instance.player;

        if (isPaused) UnpauseTheGame();
        else PauseTheGame();

    }

    private void PauseTheGame()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
        player.playerInput.Disable();
        isPaused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    private void UnpauseTheGame()
    {
        player.playerInput.Enable();
        isPaused = false;
        Time.timeScale = 1;
        pauseUI.SetActive(false);
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
