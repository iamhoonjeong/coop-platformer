using UnityEngine;

public class UISkinSelection : MonoBehaviour
{
    [SerializeField] int currentIndex;
    [SerializeField] int maxIndex;
    [SerializeField] Animator skinDisplay;

    public void SelectSkin()
    {
        SkinManager.instance.SetSkinId(currentIndex);
    }

    public void NextSkin()
    {
        currentIndex++;
        if (currentIndex > maxIndex) currentIndex = 0;

        UpdateSkinDisplay();
    }
    public void PreviousSkin()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = maxIndex;

        UpdateSkinDisplay();
    }

    void UpdateSkinDisplay()
    {
        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }

        skinDisplay.SetLayerWeight(currentIndex, 1);
    }
}
