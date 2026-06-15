using UnityEngine;

public class PlayerPrefsExample : MonoBehaviour
{
    public int fruits;
    public float seconds;
    public string levelName;

    [ContextMenu("Save value")]
    public void SaveValue()
    {
        // PlayerPrefs.SetInt("FruitsCollected", 5);
        // PlayerPrefs.SetInt("Level1Unlocked", 1);
    }

    [ContextMenu("Load value")]
    public void LoadValue()
    {
        // fruits = PlayerPrefs.GetInt("FruitsCollected");

        // bool level1Unlocked = PlayerPrefs.GetInt("Level1Unlocked", 0) == 1;
        // if (level1Unlocked) print("level is unlocked");
    }
}
