using TMPro;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    public static UIInGame instance;
    public UIFadeEffect fadeEffect { get; private set; }
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI fruitText;

    void Awake()
    {
        instance = this;

        fadeEffect = GetComponentInChildren<UIFadeEffect>();
    }

    void Start()
    {
        fadeEffect.ScreenFade(0, 1);
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
