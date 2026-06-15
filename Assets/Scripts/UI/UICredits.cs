using UnityEngine;
using UnityEngine.SceneManagement;

public class UICredits : MonoBehaviour
{
    UIFadeEffect fadeEffect;
    [SerializeField] RectTransform rectT;
    [SerializeField] float scrollSpeed = 200f;
    [SerializeField] float offScreenPosition = 1700;

    [SerializeField] string mainMenuSceneName = "MainMenu";
    bool creditsSkipped;

    void Awake()
    {
        fadeEffect = GetComponentInChildren<UIFadeEffect>();
        fadeEffect.ScreenFade(0, 2);
    }

    void Update()
    {
        rectT.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (rectT.anchoredPosition.y > offScreenPosition)
        {
            GoToMainMenu();
        }
    }

    public void SkipCredits()
    {
        if (!creditsSkipped)
        {
            scrollSpeed *= 10;
            creditsSkipped = true;
        }
        else
        {
            GoToMainMenu();
        }
    }

    void GoToMainMenu() => fadeEffect.ScreenFade(1, 1, SwitchToMenuScene);

    private void SwitchToMenuScene()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
