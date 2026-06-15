using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumberText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] TextMeshProUGUI fruitsText;

    int levelIndex;
    public string sceneName;

    public void SetupButton(int newLevelIndex)
    {
        levelIndex = newLevelIndex;
        levelNumberText.text = "Level " + levelIndex;
        sceneName = "Level_" + levelIndex;

        bestTimeText.text = TimerInfoText();
        fruitsText.text = FruitInfoText();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(sceneName);
    }

    string FruitInfoText()
    {
        int totalFruits = PlayerPrefs.GetInt("Level" + levelIndex + "TotalFruits", 0);
        string totalFruitsText = totalFruits == 0 ? "?" : totalFruits.ToString();

        int fruitsCollected = PlayerPrefs.GetInt("Level" + levelIndex + "FruisCollected");

        return "Fruits: " + fruitsCollected + " / " + totalFruitsText;
    }

    string TimerInfoText()
    {
        float timerValue = PlayerPrefs.GetFloat("Level" + levelIndex + "BestTime", 99);
        return "Best Time: " + timerValue.ToString("00");
    }
}
