using UnityEngine;

public enum BackgroundType { Blue, Brown, Gray, Green, Pink, Purple, Yellow }

public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] Vector2 movementDirection;
    MeshRenderer mesh;

    [Header("Color")]
    [SerializeField] BackgroundType backgroundType;
    [SerializeField] Texture2D[] textures;

    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }

    void Update()
    {
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }

    [ContextMenu("Update background")]
    void UpdateBackgroundTexture()
    {
        if (!mesh) mesh = GetComponent<MeshRenderer>();
        mesh.sharedMaterial.mainTexture = textures[(int)backgroundType];
    }
}
