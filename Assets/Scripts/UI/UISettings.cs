using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [SerializeField] float mixerMultiplier = 25f;

    [Header("SFX Settings")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] TextMeshProUGUI sfxSliderText;
    [SerializeField] string sfxParameter;

    [Header("BGM Settings")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] TextMeshProUGUI bgmSliderText;
    [SerializeField] string bgmParameter;

    public void SFXSlidrerValue(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
    }



    public void BGMSlidrerValue(float value)
    {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }

    void OnEnable()
    {

        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.7f);
    }
}
