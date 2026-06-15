using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public int choosenSkinId;
    public static SkinManager instance;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSkinId(int id) => choosenSkinId = id;
    public int GetSkinId() => choosenSkinId;
}
