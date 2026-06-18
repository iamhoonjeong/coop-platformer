using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[System.Serializable]
public struct Skin
{
    public string skinName;
    public int skinPrice;
    public bool unlocked;
}

public class UISkinSelection : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;
    [Space]

    DefaultInputActions defaultInput;
    UILevelSelection levelSelectionUI;
    UIMainMenu mainMenuUI;
    [SerializeField] Skin[] skinList;

    [Header("UI details")]
    [SerializeField] int skinIndex;
    [SerializeField] int maxIndex;
    [SerializeField] Animator skinDisplay;

    [SerializeField] TextMeshProUGUI buySelectText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI bankText;

    [Space]
    [SerializeField] float inputCooldown = .1f;
    float lastTimeInput;

    void Awake()
    {
        LoadSkinUnlocks();
        UpdateSkinDisplay();

        mainMenuUI = GetComponentInParent<UIMainMenu>();
        levelSelectionUI = mainMenuUI.GetComponentInChildren<UILevelSelection>(true);
        defaultInput = new DefaultInputActions();
    }

    void OnEnable()
    {
        defaultInput.Enable();
        mainMenuUI.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);

        defaultInput.UI.Navigate.performed += ctx => SwitchSkinWithNavigation(ctx);
    }

    void OnDisable()
    {
        defaultInput.Disable();
        defaultInput.UI.Navigate.performed -= ctx => SwitchSkinWithNavigation(ctx);
    }

    void SwitchSkinWithNavigation(InputAction.CallbackContext ctx)
    {
        if (Time.time - lastTimeInput < inputCooldown) return;
        if (ctx.ReadValue<Vector2>().x <= -1) PreviousSkin();
        if (ctx.ReadValue<Vector2>().x >= 1) NextSkin();
    }

    void LoadSkinUnlocks()
    {
        for (int i = 0; i < skinList.Length; i++)
        {
            string skinName = skinList[i].skinName;
            bool skinUnlocked = PlayerPrefs.GetInt(skinName + "Unlocked", 0) == 1;

            if (skinUnlocked || i == 0) skinList[i].unlocked = true;
        }
    }

    public void SelectSkin()
    {
        if (!skinList[skinIndex].unlocked) BuySkin(skinIndex);
        else
        {
            SkinManager.instance.SetSkinId(skinIndex);
            mainMenuUI.SwitchUI(levelSelectionUI.gameObject);
        }
        AudioManager.instance.PlaySFX(4);

        UpdateSkinDisplay();
    }

    public void NextSkin()
    {
        lastTimeInput = Time.time;
        skinIndex++;
        if (skinIndex > maxIndex) skinIndex = 0;

        AudioManager.instance.PlaySFX(4);
        UpdateSkinDisplay();
    }
    public void PreviousSkin()
    {
        lastTimeInput = Time.time;
        skinIndex--;
        if (skinIndex < 0) skinIndex = maxIndex;

        AudioManager.instance.PlaySFX(4);
        UpdateSkinDisplay();
    }

    void UpdateSkinDisplay()
    {
        bankText.text = "Bank: " + FruitsInBank();
        for (int i = 0; i < skinDisplay.layerCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }

        skinDisplay.SetLayerWeight(skinIndex, 1);

        if (skinList[skinIndex].unlocked)
        {
            priceText.transform.parent.gameObject.SetActive(false);
            buySelectText.text = "Select";
        }
        else
        {
            priceText.transform.parent.gameObject.SetActive(true);
            priceText.text = "Price: " + skinList[skinIndex].skinPrice;
            buySelectText.text = "Buy";
        }
    }

    void BuySkin(int index)
    {
        if (!HaveEnoughFruits(skinList[index].skinPrice))
        {
            AudioManager.instance.PlaySFX(6);
            print("not enough fruits");
            return;
        }

        AudioManager.instance.PlaySFX(10);
        string skinName = skinList[skinIndex].skinName;
        skinList[skinIndex].unlocked = true;

        PlayerPrefs.SetInt(skinName + "Unlocked", 1);
    }

    int FruitsInBank() => PlayerPrefs.GetInt("TotalFruitsAmount");

    bool HaveEnoughFruits(int price)
    {
        if (FruitsInBank() > price)
        {
            PlayerPrefs.SetInt("TotalFruitsAmount", FruitsInBank() - price);
            return true;
        }
        return false;
    }
}
