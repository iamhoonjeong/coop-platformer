using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio source")]
    [SerializeField] AudioSource[] sfx;
    [SerializeField] AudioSource[] bgm;
    [SerializeField] int bgmIndex;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (bgm.Length <= 0) return;
        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
    }

    public void PlayMusicIfNeeded()
    {
        if (bgm[bgmIndex].isPlaying == false)
        {
            PlayRandomBGM();
        }
    }

    public void PlayRandomBGM()
    {
        if (bgm.Length <= 0)
        {
            Debug.LogWarning("No BGM source");
            return;
        }
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int bgmToPlay)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }

        bgmIndex = bgmToPlay;
        bgm[bgmToPlay].Play();
    }

    public void PlaySFX(int sfxToPlay, bool randomPitch = true)
    {
        if (sfxToPlay >= sfx.Length) return;

        if (randomPitch) sfx[sfxToPlay].pitch = Random.Range(0.9f, 1.1f);

        sfx[sfxToPlay].Play();
    }

    public void StopSFX(int sfxToStop) => sfx[sfxToStop].Stop();
}
